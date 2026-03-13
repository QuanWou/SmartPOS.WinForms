using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface IStockInService
    {
        IEnumerable<StockInDTO> GetAll();

        StockInDTO GetById(int maPN);

        IEnumerable<StockInDetailDTO> GetDetailsByStockInId(int maPN);

        OperationResult Insert(StockInRequest request);
    }
}