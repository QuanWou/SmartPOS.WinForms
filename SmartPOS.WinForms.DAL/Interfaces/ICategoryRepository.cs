using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<CategoryDTO> GetAll();

        CategoryDTO GetById(int maLoai);

        CategoryDTO GetByName(string tenLoai);

        int Insert(CategoryDTO category);

        int Update(CategoryDTO category);
    }
}