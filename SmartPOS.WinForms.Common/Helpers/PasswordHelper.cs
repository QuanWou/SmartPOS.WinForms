using System;
using System.Security.Cryptography;
using System.Text;

namespace SmartPOS.WinForms.Common.Helpers
{
    public static class PasswordHelper
    {
        public static string ToSha256(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            string inputHash = ToSha256(plainTextPassword);
            return string.Equals(inputHash, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}