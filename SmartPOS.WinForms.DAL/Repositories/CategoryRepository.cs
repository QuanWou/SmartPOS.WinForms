using System.Collections.Generic;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbHelper _dbHelper;

        public CategoryRepository()
        {
            _dbHelper = new DbHelper();
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            string sql = @"
                SELECT 
                    MaLoai,
                    TenLoai,
                    MoTa,
                    TrangThai
                FROM Categories
                ORDER BY MaLoai DESC";

            return _dbHelper.Query<CategoryDTO>(sql);
        }

        public CategoryDTO GetById(int maLoai)
        {
            string sql = @"
                SELECT 
                    MaLoai,
                    TenLoai,
                    MoTa,
                    TrangThai
                FROM Categories
                WHERE MaLoai = @MaLoai";

            return _dbHelper.QueryFirstOrDefault<CategoryDTO>(sql, new { MaLoai = maLoai });
        }
        public CategoryDTO GetByName(string tenLoai)
        {
            string sql = @"
        SELECT 
            MaLoai,
            TenLoai,
            MoTa,
            TrangThai
        FROM Categories
        WHERE TenLoai = @TenLoai";

            return _dbHelper.QueryFirstOrDefault<CategoryDTO>(sql, new { TenLoai = tenLoai });
        }
        public int Insert(CategoryDTO category)
        {
            string sql = @"
                INSERT INTO Categories
                (
                    TenLoai,
                    MoTa,
                    TrangThai
                )
                VALUES
                (
                    @TenLoai,
                    @MoTa,
                    @TrangThai
                )";

            return _dbHelper.Execute(sql, category);
        }

        public int Update(CategoryDTO category)
        {
            string sql = @"
                UPDATE Categories
                SET
                    TenLoai = @TenLoai,
                    MoTa = @MoTa,
                    TrangThai = @TrangThai
                WHERE MaLoai = @MaLoai";

            return _dbHelper.Execute(sql, category);
        }
    }
}