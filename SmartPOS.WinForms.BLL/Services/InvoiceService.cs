using System;
using System.Collections.Generic;
using System.Linq;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.Common.Constants;
using SmartPOS.WinForms.Common.Helpers;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DAL.Repositories;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerOfferRepository _customerOfferRepository;

        public InvoiceService()
        {
            _invoiceRepository = new InvoiceRepository();
            _productRepository = new ProductRepository();
            _customerRepository = new CustomerRepository();
            _customerOfferRepository = new CustomerOfferRepository();
        }

        public IEnumerable<InvoiceDTO> GetAll()
        {
            return _invoiceRepository.GetAll();
        }

        public InvoiceDTO GetById(int maHD)
        {
            if (!ValidationHelper.IsPositiveInt(maHD))
            {
                return null;
            }

            return _invoiceRepository.GetById(maHD);
        }

        public IEnumerable<InvoiceDetailDTO> GetDetailsByInvoiceId(int maHD)
        {
            if (!ValidationHelper.IsPositiveInt(maHD))
            {
                return new List<InvoiceDetailDTO>();
            }

            return _invoiceRepository.GetDetailsByInvoiceId(maHD);
        }

        public CheckoutPreviewResponse PreviewCheckout(CheckoutRequest request)
        {
            try
            {
                return BuildCheckoutPreview(request);
            }
            catch (Exception ex)
            {
                return PreviewError(MessageConstants.CheckoutFailed + " " + ex.Message);
            }
        }

        public OperationResult Checkout(CheckoutRequest request)
        {
            try
            {
                CheckoutPreviewResponse preview = BuildCheckoutPreview(request);
                if (!preview.IsSuccess)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = preview.Message
                    };
                }

                int maHD = _invoiceRepository.Insert(request);

                return new OperationResult
                {
                    IsSuccess = maHD > 0,
                    Message = maHD > 0 ? MessageConstants.CheckoutSuccess : MessageConstants.CheckoutFailed,
                    DataId = maHD > 0 ? (int?)maHD : null
                };
            }
            catch (InvalidOperationException ex)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.CheckoutFailed + " " + ex.Message
                };
            }
        }

        public OperationResult UpdateStatus(int maHD, string trangThai)
        {
            if (!ValidationHelper.IsPositiveInt(maHD))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(trangThai))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Trạng thái hóa đơn không hợp lệ."
                };
            }

            if (trangThai != "Paid" && trangThai != "Cancelled")
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Trạng thái hóa đơn không hợp lệ."
                };
            }

            try
            {
                int rowsAffected = _invoiceRepository.UpdateStatus(maHD, trangThai);

                return new OperationResult
                {
                    IsSuccess = rowsAffected > 0,
                    Message = rowsAffected > 0 ? MessageConstants.UpdateSuccess : MessageConstants.UpdateFailed
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.UpdateFailed + " " + ex.Message
                };
            }
        }

        private CheckoutPreviewResponse BuildCheckoutPreview(CheckoutRequest request)
        {
            OperationResult validationResult = ValidateCheckoutRequest(request);
            if (!validationResult.IsSuccess)
            {
                return PreviewError(validationResult.Message);
            }

            decimal tongTienTruocGiam = 0;
            foreach (var item in request.ChiTietHoaDon)
            {
                ProductDTO product = _productRepository.GetById(item.MaSP);
                if (product == null || !product.TrangThai)
                {
                    return PreviewError("Sản phẩm không tồn tại hoặc đã ngừng kinh doanh.");
                }

                if (product.HanSuDung.HasValue && product.HanSuDung.Value.Date < DateTime.Today)
                {
                    return PreviewError("Sản phẩm đã hết hạn sử dụng. (" + product.TenSP + ")");
                }

                if (product.SoLuongTon < item.SoLuong)
                {
                    return PreviewError(MessageConstants.OutOfStock + " (" + product.TenSP + ")");
                }

                item.DonGiaLucBan = product.GiaBan;
                item.ThanhTien = item.SoLuong * item.DonGiaLucBan;
                tongTienTruocGiam += item.ThanhTien;
            }

            CustomerDTO customer = null;
            if (request.MaKH.HasValue)
            {
                customer = _customerRepository.GetById(request.MaKH.Value);
                if (customer == null || !customer.TrangThai)
                {
                    return PreviewError("Khách hàng không tồn tại hoặc đã ngừng hoạt động.");
                }
            }
            else
            {
                if (request.DiemSuDung > 0)
                {
                    return PreviewError("Vui lòng chọn khách hàng trước khi đổi điểm.");
                }

                if (request.MaUuDai.HasValue)
                {
                    return PreviewError("Vui lòng chọn khách hàng trước khi dùng ưu đãi.");
                }
            }

            CustomerOfferDTO offer = null;
            decimal phanTramUuDai = 0;
            decimal giamGiaUuDai = 0;

            if (request.MaUuDai.HasValue)
            {
                offer = _customerOfferRepository.GetById(request.MaUuDai.Value);
                OperationResult offerValidation = ValidateOfferForCheckout(offer, request.MaKH.Value);
                if (!offerValidation.IsSuccess)
                {
                    return PreviewError(offerValidation.Message);
                }

                phanTramUuDai = offer.PhanTramGiam;
                giamGiaUuDai = CalculateOfferDiscount(tongTienTruocGiam, phanTramUuDai);
            }

            decimal totalAfterOffer = Math.Max(0, tongTienTruocGiam - giamGiaUuDai);
            int maxRedeemPoints = 0;
            if (customer != null && totalAfterOffer > 0)
            {
                int maxByOrder = (int)Math.Floor(totalAfterOffer / LoyaltyConstants.RedeemValuePerPoint);
                maxRedeemPoints = Math.Max(0, Math.Min(customer.DiemHienCo, maxByOrder));
            }

            if (request.DiemSuDung > maxRedeemPoints)
            {
                return PreviewError("Số điểm đổi vượt quá mức có thể dùng cho hóa đơn này.");
            }

            decimal giamGiaDiem = request.DiemSuDung * LoyaltyConstants.RedeemValuePerPoint;
            decimal tongTien = Math.Max(0, totalAfterOffer - giamGiaDiem);

            return new CheckoutPreviewResponse
            {
                IsSuccess = true,
                Message = string.Empty,
                TongTienTruocGiam = tongTienTruocGiam,
                MaUuDai = offer != null ? (int?)offer.MaUuDai : null,
                PhanTramUuDai = phanTramUuDai,
                GiamGiaUuDai = giamGiaUuDai,
                DiemSuDung = request.DiemSuDung,
                GiamGiaDiem = giamGiaDiem,
                DiemToiDaCoTheDoi = maxRedeemPoints,
                TongTien = tongTien
            };
        }

        private OperationResult ValidateCheckoutRequest(CheckoutRequest request)
        {
            if (request == null)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (!ValidationHelper.IsPositiveInt(request.MaNV))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Nhân viên thanh toán không hợp lệ."
                };
            }

            if (request.MaKH.HasValue && !ValidationHelper.IsPositiveInt(request.MaKH.Value))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Khách hàng không hợp lệ."
                };
            }

            if (request.MaUuDai.HasValue && !ValidationHelper.IsPositiveInt(request.MaUuDai.Value))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Ưu đãi không hợp lệ."
                };
            }

            if (!ValidationHelper.IsNonNegativeInt(request.DiemSuDung))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Số điểm sử dụng không hợp lệ."
                };
            }

            if (request.ChiTietHoaDon == null || !request.ChiTietHoaDon.Any())
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Hóa đơn chưa có sản phẩm."
                };
            }

            foreach (var item in request.ChiTietHoaDon)
            {
                if (item == null)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = MessageConstants.InvalidInput
                    };
                }

                if (!ValidationHelper.IsPositiveInt(item.MaSP))
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Sản phẩm trong hóa đơn không hợp lệ."
                    };
                }

                if (!ValidationHelper.IsPositiveInt(item.SoLuong))
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Số lượng sản phẩm phải lớn hơn 0."
                    };
                }
            }

            return new OperationResult
            {
                IsSuccess = true,
                Message = string.Empty
            };
        }

        private OperationResult ValidateOfferForCheckout(CustomerOfferDTO offer, int maKH)
        {
            if (offer == null)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Ưu đãi không tồn tại."
                };
            }

            if (offer.MaKH != maKH)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Ưu đãi không thuộc khách hàng đã chọn."
                };
            }

            if (!offer.TrangThai)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Ưu đãi đã bị tắt."
                };
            }

            if (offer.DaSuDung)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Ưu đãi đã được sử dụng."
                };
            }

            if (offer.NgayHetHan.HasValue && offer.NgayHetHan.Value.Date < DateTime.Today)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Ưu đãi đã hết hạn."
                };
            }

            return new OperationResult { IsSuccess = true, Message = string.Empty };
        }

        private CheckoutPreviewResponse PreviewError(string message)
        {
            return new CheckoutPreviewResponse
            {
                IsSuccess = false,
                Message = message
            };
        }

        private decimal CalculateOfferDiscount(decimal subtotal, decimal percent)
        {
            if (subtotal <= 0 || percent <= 0)
            {
                return 0;
            }

            return Math.Round(subtotal * percent / 100m, 0, MidpointRounding.AwayFromZero);
        }
    }
}
