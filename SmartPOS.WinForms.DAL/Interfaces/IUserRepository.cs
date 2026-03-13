using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<UserDTO> GetAll();

        UserDTO GetById(int maNV);

        UserDTO GetByUsername(string taiKhoan);

        int Insert(UserDTO user);

        int Update(UserDTO user);
    }
}