using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartPOS.WinForms.Common.Constants;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.Common.Session
{
    public static class SessionManager
    {
        public static UserDTO CurrentUser { get; set; }

        public static bool IsLoggedIn
        {
            get { return CurrentUser != null; }
        }

        public static bool IsAdmin
        {
            get
            {
                return CurrentUser != null &&
                    string.Equals(CurrentUser.Quyen, RoleConstants.Admin, StringComparison.OrdinalIgnoreCase);
            }
        }

        public static bool IsStaff
        {
            get
            {
                return CurrentUser != null &&
                    string.Equals(CurrentUser.Quyen, RoleConstants.Staff, StringComparison.OrdinalIgnoreCase);
            }
        }

        public static void Clear()
        {
            CurrentUser = null;
        }
    }
}
