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
    public class StockInService : IStockInService
    {
        private readonly IStockInRepository _stockInRepository;
        private readonly IProductRepository _productRepository;

        public StockInService()
        {
            _stockInRepository = new StockInRepository();
            _productRepository = new ProductRepository();
        }

        public IEnumerable<StockInDTO> GetAll()
        {
            return _stockInRepository.GetAll();
        }

        public StockInDTO GetById(int maPN)
        {
            if (!ValidationHelper.IsPositiveInt(maPN))
            {
                return null;
            }

            return _stockInRepository.GetById(maPN);
        }

        public IEnumerable<StockInDetailDTO> GetDetailsByStockInId(int maPN)
        {
            if (!ValidationHelper.IsPositiveInt(maPN))
            {
                return new List<StockInDetailDTO>();
            }

            return _stockInRepository.GetDetailsByStockInId(maPN);
        }

        public OperationResult Insert(StockInRequest request)
        {
            var validationResult = ValidateStockInRequest(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                foreach (var item in request.ChiTietNhapKho)
                {
                    ProductDTO product = _productRepository.GetById(item.MaSP);
                    if (product == null)
                    {
                        return new OperationResult
                        {
                            IsSuccess = false,
                            Message = "Sản phẩm không tồn tại."
                        };
                    }

                    item.ThanhTien = item.SoLuong * item.GiaNhapLucNhap;
                }

                int maPN = _stockInRepository.Insert(request);

                return new OperationResult
                {
                    IsSuccess = maPN > 0,
                    Message = maPN > 0 ? MessageConstants.SaveSuccess : MessageConstants.SaveFailed,
                    DataId = maPN > 0 ? (int?)maPN : null
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.SaveFailed + " " + ex.Message
                };
            }
        }

        private OperationResult ValidateStockInRequest(StockInRequest request)
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
                    Message = "Nhân viên nhập kho không hợp lệ."
                };
            }

            if (request.ChiTietNhapKho == null || !request.ChiTietNhapKho.Any())
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Phiếu nhập chưa có sản phẩm."
                };
            }

            foreach (var item in request.ChiTietNhapKho)
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
                        Message = "Sản phẩm trong phiếu nhập không hợp lệ."
                    };
                }

                if (!ValidationHelper.IsPositiveInt(item.SoLuong))
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Số lượng nhập phải lớn hơn 0."
                    };
                }

                if (!ValidationHelper.IsNonNegativeDecimal(item.GiaNhapLucNhap))
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Giá nhập không hợp lệ."
                    };
                }

                if (item.HanSuDung.HasValue && item.HanSuDung.Value.Date < DateTime.Today)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Hạn sử dụng của lô nhập phải từ hôm nay trở đi."
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
