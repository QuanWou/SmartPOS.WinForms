using System;
using System.Collections.Generic;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.Common.Constants;
using SmartPOS.WinForms.Common.Helpers;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DAL.Repositories;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService()
        {
            _categoryRepository = new CategoryRepository();
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public CategoryDTO GetById(int maLoai)
        {
            if (!ValidationHelper.IsPositiveInt(maLoai))
            {
                return null;
            }

            return _categoryRepository.GetById(maLoai);
        }

        public OperationResult Insert(CategoryDTO category)
        {
            var validationResult = ValidateCategory(category, false);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                category.TenLoai = category.TenLoai.Trim();
                category.MoTa = string.IsNullOrWhiteSpace(category.MoTa) ? null : category.MoTa.Trim();
                CategoryDTO existingCategory = _categoryRepository.GetByName(category.TenLoai);
                if (existingCategory != null)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Tên danh mục đã tồn tại."
                    };
                }

                int rowsAffected = _categoryRepository.Insert(category);

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

        public OperationResult Update(CategoryDTO category)
        {
            var validationResult = ValidateCategory(category, true);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                category.TenLoai = category.TenLoai.Trim();
                category.MoTa = string.IsNullOrWhiteSpace(category.MoTa) ? null : category.MoTa.Trim();
                CategoryDTO existingCategory = _categoryRepository.GetByName(category.TenLoai);
                if (existingCategory != null && existingCategory.MaLoai != category.MaLoai)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Tên danh mục đã tồn tại."
                    };
                }

                int rowsAffected = _categoryRepository.Update(category);

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

        private OperationResult ValidateCategory(CategoryDTO category, bool isUpdate)
        {
            if (category == null)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (isUpdate && !ValidationHelper.IsPositiveInt(category.MaLoai))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(category.TenLoai))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Tên danh mục không được để trống."
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