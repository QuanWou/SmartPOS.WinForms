using System;
using System.Collections.Generic;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.Common.Constants;
using SmartPOS.WinForms.Common.Helpers;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DAL.Repositories;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }

        public IEnumerable<CustomerDTO> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public CustomerDTO GetById(int maKH)
        {
            if (!ValidationHelper.IsPositiveInt(maKH))
            {
                return null;
            }

            return _customerRepository.GetById(maKH);
        }

        public CustomerDTO GetByPhone(string soDienThoai)
        {
            if (ValidationHelper.IsNullOrWhiteSpace(soDienThoai))
            {
                return null;
            }

            return _customerRepository.GetByPhone(soDienThoai.Trim());
        }

        public IEnumerable<CustomerDTO> Search(CustomerSearchRequest request)
        {
            if (request == null)
            {
                request = new CustomerSearchRequest();
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                request.Keyword = request.Keyword.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.HangThanhVien))
            {
                request.HangThanhVien = request.HangThanhVien.Trim();
            }

            return _customerRepository.Search(request);
        }

        public OperationResult Insert(CustomerDTO customer)
        {
            var validationResult = ValidateCustomer(customer, false);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                customer.HoTen = customer.HoTen.Trim();
                customer.SoDienThoai = customer.SoDienThoai.Trim();
                customer.DiaChi = string.IsNullOrWhiteSpace(customer.DiaChi) ? null : customer.DiaChi.Trim();

                CustomerDTO existingCustomer = _customerRepository.GetByPhone(customer.SoDienThoai);
                if (existingCustomer != null)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Số điện thoại khách hàng đã tồn tại."
                    };
                }

                customer.NgayThamGia = DateTime.Now;
                customer.HangThanhVien = ResolveMemberRank(customer.TongChiTieu, customer.SoLanMua);
                customer.TongChiTieu = 0;
                customer.SoLanMua = 0;
                customer.DiemHienCo = 0;
                customer.TongDiemDaDoi = 0;
                customer.TrangThai = true;
                customer.NgayCapNhat = null;

                int maKH = _customerRepository.Insert(customer);

                return new OperationResult
                {
                    IsSuccess = maKH > 0,
                    DataId = maKH > 0 ? (int?)maKH : null,
                    Message = maKH > 0 ? MessageConstants.SaveSuccess : MessageConstants.SaveFailed
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

        public OperationResult Update(CustomerDTO customer)
        {
            var validationResult = ValidateCustomer(customer, true);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                CustomerDTO currentCustomer = _customerRepository.GetById(customer.MaKH);
                if (currentCustomer == null)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Khách hàng không tồn tại."
                    };
                }

                customer.HoTen = customer.HoTen.Trim();
                customer.SoDienThoai = customer.SoDienThoai.Trim();
                customer.DiaChi = string.IsNullOrWhiteSpace(customer.DiaChi) ? null : customer.DiaChi.Trim();

                CustomerDTO existingCustomer = _customerRepository.GetByPhone(customer.SoDienThoai);
                if (existingCustomer != null && existingCustomer.MaKH != customer.MaKH)
                {
                    return new OperationResult
                    {
                        IsSuccess = false,
                        Message = "Số điện thoại khách hàng đã tồn tại."
                    };
                }

                customer.HangThanhVien = ResolveMemberRank(currentCustomer.TongChiTieu, currentCustomer.SoLanMua);

                int rowsAffected = _customerRepository.Update(customer);

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

        public OperationResult UpdateStatus(int maKH, bool trangThai)
        {
            if (!ValidationHelper.IsPositiveInt(maKH))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            try
            {
                int rowsAffected = _customerRepository.UpdateStatus(maKH, trangThai);
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

        public CustomerStatsResponse GetStats()
        {
            return _customerRepository.GetStats();
        }

        public IEnumerable<CustomerPurchaseHistoryResponse> GetPurchaseHistory(int maKH)
        {
            if (!ValidationHelper.IsPositiveInt(maKH))
            {
                return new List<CustomerPurchaseHistoryResponse>();
            }

            return _customerRepository.GetPurchaseHistory(maKH);
        }

        public IEnumerable<CustomerPointTransactionDTO> GetPointHistory(int maKH)
        {
            if (!ValidationHelper.IsPositiveInt(maKH))
            {
                return new List<CustomerPointTransactionDTO>();
            }

            return _customerRepository.GetPointHistory(maKH);
        }

        public IEnumerable<CustomerCategoryTrendResponse> GetCategoryTrends(int maKH)
        {
            if (!ValidationHelper.IsPositiveInt(maKH))
            {
                return new List<CustomerCategoryTrendResponse>();
            }

            return _customerRepository.GetCategoryTrends(maKH);
        }

        public IEnumerable<CustomerTopProductResponse> GetTopProducts(int maKH)
        {
            if (!ValidationHelper.IsPositiveInt(maKH))
            {
                return new List<CustomerTopProductResponse>();
            }

            return _customerRepository.GetTopProducts(maKH);
        }

        public int CalculateEarnedPoints(decimal amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            return (int)Math.Floor(amount / LoyaltyConstants.EarnAmountPerPoint);
        }

        public decimal CalculateRedeemValue(int points)
        {
            if (points <= 0)
            {
                return 0;
            }

            return points * LoyaltyConstants.RedeemValuePerPoint;
        }

        public string ResolveMemberRank(decimal totalSpend, int purchaseCount)
        {
            if (totalSpend >= 15000000 || purchaseCount >= 15)
            {
                return LoyaltyConstants.RankPlatinum;
            }

            if (totalSpend >= 5000000 || purchaseCount >= 5)
            {
                return LoyaltyConstants.RankGold;
            }

            if (totalSpend >= 1000000 || purchaseCount >= 2)
            {
                return LoyaltyConstants.RankSilver;
            }

            return LoyaltyConstants.RankMember;
        }

        public OperationResult ValidatePointRedemption(int? maKH, int points, decimal orderTotal)
        {
            if (points < 0 || orderTotal < 0)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (points == 0)
            {
                return new OperationResult { IsSuccess = true, Message = string.Empty };
            }

            if (!maKH.HasValue || maKH.Value <= 0)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Vui lòng chọn khách hàng trước khi đổi điểm."
                };
            }

            CustomerDTO customer = _customerRepository.GetById(maKH.Value);
            if (customer == null || !customer.TrangThai)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Khách hàng không tồn tại hoặc đã ngừng hoạt động."
                };
            }

            if (points > customer.DiemHienCo)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Số điểm đổi vượt quá điểm hiện có của khách hàng."
                };
            }

            if (CalculateRedeemValue(points) > orderTotal)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Giá trị điểm đổi không được vượt quá tổng tiền đơn hàng."
                };
            }

            return new OperationResult { IsSuccess = true, Message = string.Empty };
        }

        private OperationResult ValidateCustomer(CustomerDTO customer, bool isUpdate)
        {
            if (customer == null)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (isUpdate && !ValidationHelper.IsPositiveInt(customer.MaKH))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = MessageConstants.InvalidInput
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(customer.HoTen))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Tên khách hàng không được để trống."
                };
            }

            if (ValidationHelper.IsNullOrWhiteSpace(customer.SoDienThoai))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Số điện thoại không được để trống."
                };
            }

            return new OperationResult { IsSuccess = true, Message = string.Empty };
        }
    }
}
