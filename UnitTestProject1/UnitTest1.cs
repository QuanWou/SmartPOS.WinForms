using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartPOS.WinForms.BLL.Services; // Thay bằng namespace thực tế của bạn
using SmartPOS.WinForms.DTO.Requests;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetHashToUpdateDB()
        {
            // Nhập mật khẩu bạn muốn hash ở đây
            string password = "admin123";
            string hash = SmartPOS.WinForms.Common.Helpers.PasswordHelper.ToSha256(password);

            // In ra cửa sổ Output của Test
            System.Diagnostics.Debug.WriteLine($"PASSWORD: {password}");
            System.Diagnostics.Debug.WriteLine($"HASH: {hash}");
        }
    }
}