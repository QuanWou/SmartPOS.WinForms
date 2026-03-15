using System.Collections.Generic;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.Common.Helpers;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DAL.Repositories;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.BLL.Services
{
    public class ProductLotService : IProductLotService
    {
        private readonly IProductLotRepository _productLotRepository;

        public ProductLotService()
        {
            _productLotRepository = new ProductLotRepository();
        }

        public IEnumerable<ProductLotDTO> GetAll()
        {
            return _productLotRepository.GetAll();
        }

        public IEnumerable<ProductLotDTO> GetByProductId(int maSP)
        {
            if (!ValidationHelper.IsPositiveInt(maSP))
            {
                return new List<ProductLotDTO>();
            }

            return _productLotRepository.GetByProductId(maSP);
        }

        public IEnumerable<ProductLotDTO> GetExpiringLots(int days)
        {
            if (days < 0)
            {
                days = 0;
            }

            return _productLotRepository.GetExpiringLots(days);
        }
    }
}
