using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<ProductDTO> GetAll();

        ProductDTO GetById(int maSP);

        ProductDTO GetByBarcode(string maVach);

        IEnumerable<ProductDTO> Search(ProductSearchRequest request);

        int Insert(ProductDTO product);

        int Update(ProductDTO product);

        bool HasTransactionHistory(int maSP);

        int Delete(int maSP);

        int UpdateStatus(int maSP, bool trangThai);

        int UpdateStock(int maSP, int soLuongTonMoi);
    }
}
