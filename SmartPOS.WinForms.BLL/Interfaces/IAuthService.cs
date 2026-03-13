using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface IAuthService
    {
        LoginResponse Login(LoginRequest request);
    }
}