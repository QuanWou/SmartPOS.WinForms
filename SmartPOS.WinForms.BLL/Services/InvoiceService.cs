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

        public InvoiceService()
        {
            _invoiceRepository = new InvoiceRepository();
            _productRepository = new ProductRepository();
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

        public OperationResult Checkout(CheckoutRequest request)
        {
            var validationResult = ValidateCheckoutRequest(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                foreach (var item in request.ChiTietHoaDon)
                {
                    ProductDTO product = _productRepository.GetById(item.MaSP);
                    if (product == null || !product.TrangThai)
                    {
                        return new OperationResult
                        {
                            IsSuccess = false,
                            Message = "Sản phẩm không tồn tại hoặc đã ngừng kinh doanh."
                        };
                    }

                    if (product.SoLuongTon < item.SoLuong)
                    {
                        return new OperationResult
                        {
                            IsSuccess = false,
                            Message = MessageConstants.OutOfStock + " (" + product.TenSP + ")"
                        };
                    }

                    item.DonGiaLucBan = product.GiaBan;
                    item.ThanhTien = item.SoLuong * item.DonGiaLucBan;
                }

                int maHD = _invoiceRepository.Insert(request);

                return new OperationResult
                {
                    IsSuccess = maHD > 0,
                    Message = maHD > 0 ? MessageConstants.CheckoutSuccess : MessageConstants.CheckoutFailed,
                    DataId = maHD > 0 ? (int?)maHD : null
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
    }
}