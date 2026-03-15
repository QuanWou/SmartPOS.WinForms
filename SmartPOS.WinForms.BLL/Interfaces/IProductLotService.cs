using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface IProductLotService
    {
        IEnumerable<ProductLotDTO> GetAll();

        IEnumerable<ProductLotDTO> GetByProductId(int maSP);

        IEnumerable<ProductLotDTO> GetExpiringLots(int days);
    }
}
