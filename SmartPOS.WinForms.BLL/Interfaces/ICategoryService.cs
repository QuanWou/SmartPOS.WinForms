using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDTO> GetAll();

        CategoryDTO GetById(int maLoai);

        OperationResult Insert(CategoryDTO category);

        OperationResult Update(CategoryDTO category);
    }
}