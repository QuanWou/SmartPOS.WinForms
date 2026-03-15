using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface IProductLotRepository
    {
        IEnumerable<ProductLotDTO> GetAll();

        IEnumerable<ProductLotDTO> GetByProductId(int maSP);

        IEnumerable<ProductLotDTO> GetExpiringLots(int days);
    }
}
