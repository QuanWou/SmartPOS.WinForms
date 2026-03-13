using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface IStockInRepository
    {
        IEnumerable<StockInDTO> GetAll();

        StockInDTO GetById(int maPN);

        IEnumerable<StockInDetailDTO> GetDetailsByStockInId(int maPN);

        int Insert(StockInRequest request);
    }
}