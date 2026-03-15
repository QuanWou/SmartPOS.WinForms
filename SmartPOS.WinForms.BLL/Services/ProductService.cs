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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            return _productRepository.GetAll();
        }

        public ProductDTO GetById(int maSP)
        {
            if (!ValidationHelper.IsPositiveInt(maSP))
            {
                return null;
            }

            return _productRepository.GetById(maSP);
        }

        public ProductDTO GetByBarcode(string maVach)
        {
            if (ValidationHelper.IsNullOrWhiteSpace(maVach))
            {
                return null;
            }

            return _productRepository.GetByBarcode(maVach.Trim());
        }

        public IEnumerable<ProductDTO> Search(ProductSearchRequest request)
        {
            if (request == null)
            {
                request = new ProductSearchRequest();
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                request.Keyword = request.Keyword.Trim();
            }

            return _productRepository.Search(request);
        }

        public OperationResult Insert(ProductDTO product)
        {
            var validationResult = ValidateProduct(product, false);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                product.TenSP = product.TenSP.Trim();
                product.MaVach = product.MaVach.Trim();
                product.DonViTinh = product.DonViTinh.Trim();
                product.HinhAnh = string.IsNullOrWhiteSpace(product.HinhAnh) ? null : product.HinhAnh.Trim();
                product.MoTa = string.IsNullOrWhiteSpace(product.MoTa) ? null : product.MoTa.Trim();
                if (IsBarcodeDuplicated(product.MaVach))
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Mã vạch đã tồn tại."
                    };
                }
                product.NgayTao = DateTime.Now;
                product.NgayCapNhat = null;

                int rowsAffected = _productRepository.Insert(product);

                return new OperationResult
                {
                    IsSuccess = rowsAffected > 0,
                    Message = rowsAffected > 0 ? MessageConstants.SaveSuccess : MessageConstants.SaveFailed
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

        public OperationResult Update(ProductDTO product)
        {
            var validationResult = ValidateProduct(product, true);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                product.TenSP = product.TenSP.Trim();
                product.MaVach = product.MaVach.Trim();
                product.DonViTinh = product.DonViTinh.Trim();
                product.HinhAnh = string.IsNullOrWhiteSpace(product.HinhAnh) ? null : product.HinhAnh.Trim();
                product.MoTa = string.IsNullOrWhiteSpace(product.MoTa) ? null : product.MoTa.Trim();
                if (IsBarcodeDuplicated(product.MaVach, product.MaSP))
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Mã vạch đã tồn tại."
                    };
                }
                product.NgayCapNhat = DateTime.Now;

                int rowsAffected = _productRepository.Update(product);

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

        public OperationResult UpdateStock(int maSP, int soLuongTonMoi)
        {
            if (!ValidationHelper.IsPositiveInt(maSP))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (!ValidationHelper.IsNonNegativeInt(soLuongTonMoi))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Số lượng tồn mới không hợp lệ."
                };
            }

            try
            {
                int rowsAffected = _productRepository.UpdateStock(maSP, soLuongTonMoi);

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
        private bool IsBarcodeDuplicated(string maVach, int currentMaSP = 0)
        {
            if (ValidationHelper.IsNullOrWhiteSpace(maVach))
            {
                return false;
            }

            string barcode = maVach.Trim();
            ProductDTO existingProduct = _productRepository.GetAll()
                .FirstOrDefault(x =>
                    !ValidationHelper.IsNullOrWhiteSpace(x.MaVach) &&
                    string.Equals(x.MaVach.Trim(), barcode, StringComparison.OrdinalIgnoreCase));
            if (existingProduct == null)
            {
                return false;
            }

            return existingProduct.MaSP != currentMaSP;
        }
        private OperationResult ValidateProduct(ProductDTO product, bool isUpdate)
        {
            if (product == null)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (isUpdate && !ValidationHelper.IsPositiveInt(product.MaSP))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(product.TenSP))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Tên sản phẩm không được để trống."
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(product.MaVach))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Mã vạch không được để trống."
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(product.DonViTinh))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Đơn vị tính không được để trống."
                };
            }

            if (!ValidationHelper.IsNonNegativeDecimal(product.GiaNhap))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Giá nhập không hợp lệ."
                };
            }

            if (!ValidationHelper.IsNonNegativeDecimal(product.GiaBan))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Giá bán không hợp lệ."
                };
            }

            if (!ValidationHelper.IsNonNegativeInt(product.SoLuongTon))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Số lượng tồn không hợp lệ."
                };
            }

            if (!ValidationHelper.IsPositiveInt(product.MaLoai))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Loại sản phẩm không hợp lệ."
                };
            }

            return new OperationResult
            {
                IsSuccess = true,
                Message = string.Empty
            };
        }
    }
}
