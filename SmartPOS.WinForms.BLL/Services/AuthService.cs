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
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService()
        {
            _userRepository = new UserRepository();
        }

        public LoginResponse Login(LoginRequest request)
        {
            if (request == null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput,
                    User = null
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(request.TaiKhoan))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = MessageConstants.EmptyUsername,
                    User = null
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(request.MatKhau))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = MessageConstants.EmptyPassword,
                    User = null
                };
            }

            UserDTO user = _userRepository.GetByUsername(request.TaiKhoan.Trim());

            if (user == null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = MessageConstants.LoginFailed,
                    User = null
                };
            }

            if (!user.TrangThai)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Tài khoản đã bị khóa.",
                    User = null
                };
            }

            bool isValidPassword = PasswordHelper.VerifyPassword(
                request.MatKhau.Trim(),
                user.MatKhauHash
            );

            if (!isValidPassword)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = MessageConstants.LoginFailed,
                    User = null
                };
            }

            return new LoginResponse
            {
                IsSuccess = true,
                Message = MessageConstants.LoginSuccess,
                User = user
            };
        }
    }
}