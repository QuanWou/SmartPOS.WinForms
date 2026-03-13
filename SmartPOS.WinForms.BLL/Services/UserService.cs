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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public IEnumerable<UserDTO> GetAll()
        {
            return _userRepository.GetAll();
        }

        public UserDTO GetById(int maNV)
        {
            if (!ValidationHelper.IsPositiveInt(maNV))
            {
                return null;
            }

            return _userRepository.GetById(maNV);
        }

        public UserDTO GetByUsername(string taiKhoan)
        {
            if (ValidationHelper.IsNullOrWhiteSpace(taiKhoan))
            {
                return null;
            }

            return _userRepository.GetByUsername(taiKhoan.Trim());
        }

        public OperationResult Insert(UserDTO user)
        {
            var validationResult = ValidateUser(user, false);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                user.TenNV = user.TenNV.Trim();
                user.TaiKhoan = user.TaiKhoan.Trim();
                user.SoDienThoai = string.IsNullOrWhiteSpace(user.SoDienThoai) ? null : user.SoDienThoai.Trim();
                user.DiaChi = string.IsNullOrWhiteSpace(user.DiaChi) ? null : user.DiaChi.Trim();
                UserDTO existingUser = _userRepository.GetByUsername(user.TaiKhoan);
                if (existingUser != null)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Tài khoản đã tồn tại."
                    };
                }
                user.MatKhauHash = PasswordHelper.ToSha256(user.MatKhauHash);
                user.NgayTao = DateTime.Now;

                int rowsAffected = _userRepository.Insert(user);

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

        public OperationResult Update(UserDTO user)
        {
            var validationResult = ValidateUser(user, true);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                user.TenNV = user.TenNV.Trim();
                user.TaiKhoan = user.TaiKhoan.Trim();
                user.SoDienThoai = string.IsNullOrWhiteSpace(user.SoDienThoai) ? null : user.SoDienThoai.Trim();
                user.DiaChi = string.IsNullOrWhiteSpace(user.DiaChi) ? null : user.DiaChi.Trim();
                UserDTO existingUser = _userRepository.GetByUsername(user.TaiKhoan);
                if (existingUser != null && existingUser.MaNV != user.MaNV)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Tài khoản đã tồn tại."
                    };
                }

                if (!string.IsNullOrWhiteSpace(user.MatKhauHash))
                {
                    user.MatKhauHash = PasswordHelper.ToSha256(user.MatKhauHash);
                }

                int rowsAffected = _userRepository.Update(user);

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

        private OperationResult ValidateUser(UserDTO user, bool isUpdate)
        {
            if (user == null)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (isUpdate && !ValidationHelper.IsPositiveInt(user.MaNV))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(user.TenNV))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Tên nhân viên không được để trống."
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(user.TaiKhoan))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Tài khoản không được để trống."
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(user.Quyen))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Quyền không được để trống."
                };
            }

            if (user.Quyen != RoleConstants.Admin && user.Quyen != RoleConstants.Staff)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Quyền người dùng không hợp lệ."
                };
            }

            if (!isUpdate && ValidationHelper.IsNullOrWhiteSpace(user.MatKhauHash))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Mật khẩu không được để trống."
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