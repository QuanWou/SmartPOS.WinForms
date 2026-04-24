using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductDTO> GetAll();

        ProductDTO GetById(int maSP);

        ProductDTO GetByBarcode(string maVach);

        IEnumerable<ProductDTO> Search(ProductSearchRequest request);

        OperationResult Insert(ProductDTO product);

        OperationResult Update(ProductDTO product);

        OperationResult Delete(int maSP);

        OperationResult UpdateStock(int maSP, int soLuongTonMoi);
    }
}
