using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuLich.DatabaseUtils
{
    internal class UserQuery
    {
        static SqlConnection sqlcon = SystemQuery.sqlcon;

        public static DataTable getDSChuyenDi()
        {
            DataTable dataTable = new DataTable();

                string query = @"
                SELECT cd.ID_ChuyenDi, cd.TenChuyenDi, cd.HanhTrinh, lt.NgayBatDau, 
                       cd.SoNgayDi, cd.SoLuong, cd.Gia, 
                       ISNULL(AVG(dg.SoSao), 0) AS SoSao
                FROM ChuyenDi cd
                JOIN LichTrinh lt ON cd.ID_ChuyenDi = lt.ID_ChuyenDi
                LEFT JOIN DanhGia dg ON cd.ID_ChuyenDi = dg.ID_ChuyenDi
                GROUP BY cd.ID_ChuyenDi, cd.TenChuyenDi, cd.HanhTrinh, lt.NgayBatDau, 
                         cd.SoNgayDi, cd.SoLuong, cd.Gia";

                SqlCommand cmd = new SqlCommand(query, sqlcon);

            try
            {
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            finally 
            {
                sqlcon.Close();
            }
            

            return dataTable;
            
        }
    }
}
