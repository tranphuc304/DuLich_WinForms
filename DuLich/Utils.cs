using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DuLich
{
    class Utils
    {

        public static bool IsValidEmail(string input)
        {
            // Standard email validation pattern
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(input, emailPattern);
        }

        public static string GenerateSecurePassword(int length)
        {
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string special = "@#$%^&*!";
            const string allChars = lower + upper + digits + special;

            Random random = new Random();
            StringBuilder password = new StringBuilder();

            // Đảm bảo có ít nhất một ký tự từ mỗi nhóm
            password.Append(lower[random.Next(lower.Length)]);
            password.Append(upper[random.Next(upper.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(special[random.Next(special.Length)]);

            // Điền các ký tự còn lại ngẫu nhiên
            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Xáo trộn thứ tự mật khẩu để không có mẫu dự đoán được
            return new string(password.ToString().ToCharArray().OrderBy(x => random.Next()).ToArray());
        }
    }
}
