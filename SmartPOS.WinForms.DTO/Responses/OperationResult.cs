using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public int? DataId { get; set; }
        public string Message { get; set; }
    }
}
