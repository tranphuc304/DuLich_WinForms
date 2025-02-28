using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public static async void CheckAndColorizeDataGridView(DataGridView dgv)
        {
            await Task.Delay(500); // Delay 500ms để đảm bảo dữ liệu đã load đầy đủ

            // Tìm cột chứa "Trạng Thái"
            DataGridViewColumn targetColumn = null;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.HeaderText.Contains("Trạng Thái"))
                {
                    targetColumn = col;
                    break;
                }
            }

            // Nếu không tìm thấy cột phù hợp, thoát hàm
            if (targetColumn == null) return;

            // Duyệt từng hàng và tô màu
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var cell = row.Cells[targetColumn.Index];
                if (cell.Value != null)
                {
                    string status = RemoveDiacritics(cell.Value.ToString().Trim());

                    if (status.Equals("Da Thanh Toan", StringComparison.OrdinalIgnoreCase))
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                }
            }
        }

        private static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            Regex regex = new Regex(@"\p{M}");
            return regex.Replace(normalizedString, "").Replace("Đ", "D").Replace("đ", "d").Replace("Ð", "D");
        }
    }
}
