using System.Collections.Generic;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbHelper _dbHelper;

        public UserRepository()
        {
            _dbHelper = new DbHelper();
        }

        public IEnumerable<UserDTO> GetAll()
        {
            string sql = @"
                SELECT
                    MaNV,
                    TenNV,
                    TaiKhoan,
                    MatKhauHash,
                    Quyen,
                    SoDienThoai,
                    DiaChi,
                    TrangThai,
                    NgayTao
                FROM Users
                ORDER BY MaNV DESC";

            return _dbHelper.Query<UserDTO>(sql);
        }

        public UserDTO GetById(int maNV)
        {
            string sql = @"
                SELECT
                    MaNV,
                    TenNV,
                    TaiKhoan,
                    MatKhauHash,
                    Quyen,
                    SoDienThoai,
                    DiaChi,
                    TrangThai,
                    NgayTao
                FROM Users
                WHERE MaNV = @MaNV";

            return _dbHelper.QueryFirstOrDefault<UserDTO>(sql, new { MaNV = maNV });
        }

        public UserDTO GetByUsername(string taiKhoan)
        {
            string sql = @"
                SELECT
                    MaNV,
                    TenNV,
                    TaiKhoan,
                    MatKhauHash,
                    Quyen,
                    SoDienThoai,
                    DiaChi,
                    TrangThai,
                    NgayTao
                FROM Users
                WHERE TaiKhoan = @TaiKhoan";

            return _dbHelper.QueryFirstOrDefault<UserDTO>(sql, new { TaiKhoan = taiKhoan });
        }

        public int Insert(UserDTO user)
        {
            string sql = @"
                INSERT INTO Users
                (
                    TenNV,
                    TaiKhoan,
                    MatKhauHash,
                    Quyen,
                    SoDienThoai,
                    DiaChi,
                    TrangThai,
                    NgayTao
                )
                VALUES
                (
                    @TenNV,
                    @TaiKhoan,
                    @MatKhauHash,
                    @Quyen,
                    @SoDienThoai,
                    @DiaChi,
                    @TrangThai,
                    @NgayTao
                )";

            return _dbHelper.Execute(sql, user);
        }

        public int Update(UserDTO user)
        {
            string sql = @"
                UPDATE Users
                SET
                    TenNV = @TenNV,
                    TaiKhoan = @TaiKhoan,
                    MatKhauHash = @MatKhauHash,
                    Quyen = @Quyen,
                    SoDienThoai = @SoDienThoai,
                    DiaChi = @DiaChi,
                    TrangThai = @TrangThai
                WHERE MaNV = @MaNV";

            return _dbHelper.Execute(sql, user);
        }
    }
}