using System;
using System.Collections.Generic;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.Common.Constants;
using SmartPOS.WinForms.Common.Helpers;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DAL.Repositories;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Services
{
    public class CustomerOfferService : ICustomerOfferService
    {
        private readonly ICustomerOfferRepository _customerOfferRepository;
        private readonly ICustomerRepository _customerRepository;

        public CustomerOfferService()
        {
            _customerOfferRepository = new CustomerOfferRepository();
            _customerRepository = new CustomerRepository();
        }

        public CustomerOfferDTO GetById(int maUuDai)
        {
            if (!ValidationHelper.IsPositiveInt(maUuDai))
            {
                return null;
            }

            return _customerOfferRepository.GetById(maUuDai);
        }

        public IEnumerable<CustomerOfferDTO> GetByCustomerId(int maKH)
        {
            if (!ValidationHelper.IsPositiveInt(maKH))
            {
                return new List<CustomerOfferDTO>();
            }

            return _customerOfferRepository.GetByCustomerId(maKH);
        }

        public IEnumerable<CustomerOfferDTO> GetAvailableByCustomerId(int maKH)
        {
            if (!ValidationHelper.IsPositiveInt(maKH))
            {
                return new List<CustomerOfferDTO>();
            }

            return _customerOfferRepository.GetAvailableByCustomerId(maKH);
        }

        public OperationResult Insert(CustomerOfferDTO offer)
        {
            OperationResult validation = ValidateOffer(offer);
            if (!validation.IsSuccess)
            {
                return validation;
            }

            try
            {
                offer.TenUuDai = offer.TenUuDai.Trim();
                offer.GhiChu = string.IsNullOrWhiteSpace(offer.GhiChu) ? null : offer.GhiChu.Trim();
                offer.TrangThai = true;
                offer.DaSuDung = false;

                int maUuDai = _customerOfferRepository.Insert(offer);
                return new OperationResult
                {
                    IsSuccess = maUuDai > 0,
                    DataId = maUuDai > 0 ? (int?)maUuDai : null,
                    Message = maUuDai > 0 ? MessageConstants.SaveSuccess : MessageConstants.SaveFailed
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.SaveFailed + " " + ex.Message
                };
            }
        }

        public OperationResult UpdateStatus(int maUuDai, bool trangThai)
        {
            if (!ValidationHelper.IsPositiveInt(maUuDai))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            try
            {
                int rowsAffected = _customerOfferRepository.UpdateStatus(maUuDai, trangThai);
                return new OperationResult
                {
                    IsSuccess = rowsAffected > 0,
                    Message = rowsAffected > 0 ? MessageConstants.UpdateSuccess : MessageConstants.UpdateFailed
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.UpdateFailed + " " + ex.Message
                };
            }
        }

        private OperationResult ValidateOffer(CustomerOfferDTO offer)
        {
            if (offer == null || !ValidationHelper.IsPositiveInt(offer.MaKH))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            CustomerDTO customer = _customerRepository.GetById(offer.MaKH);
            if (customer == null || !customer.TrangThai)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Khach hang khong ton tai hoac da ngung hoat dong."
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(offer.TenUuDai))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Ten uu dai khong duoc de trong."
                };
            }

            if (offer.PhanTramGiam <= 0 || offer.PhanTramGiam > 100)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Phan tram uu dai phai tu 1 den 100."
                };
            }

            if (offer.NgayHetHan.HasValue && offer.NgayHetHan.Value.Date < DateTime.Today)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Han dung uu dai khong duoc nho hon ngay hien tai."
                };
            }

            return new OperationResult { IsSuccess = true, Message = string.Empty };
        }
    }
}
