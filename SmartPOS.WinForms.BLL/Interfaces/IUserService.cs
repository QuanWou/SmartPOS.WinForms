using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAll();

        UserDTO GetById(int maNV);

        UserDTO GetByUsername(string taiKhoan);

        OperationResult Insert(UserDTO user);

        OperationResult Update(UserDTO user);
    }
}