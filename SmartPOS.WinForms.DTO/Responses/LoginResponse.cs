using SmartPOS.WinForms.DTO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public UserDTO User { get; set; }
    }
}
