using Luman.Busines.DTOs.ProductDTO;
using Luman.DataLayer.Context;
using Luman.DataLayer.EntityModel.Product;
using Luman.DataLayer.EntityModel.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luman.Busines.Services.ProductService
{
    public class ProductService : IProductService
    {

        private readonly LumanContext _context;

        public ProductService(LumanContext context)
        {
            _context = context;
        }

        public bool AddCategory(Category model)
        {
            _context.categories.Add(model);
            return Save();
        }

        public void addgroup(Product product, int groupid)
        {

            CategoryProduct categoryProduct = new()
            {
                ProductId = product.ProductId,
                CategoryId = groupid
            };
            _context.Add(categoryProduct);
            Save();
        }

        public void AddToFavorite(int proid, int userid)
        {

            // افزودن به دیتابیس
            FavoriteProduct favoriteProduct = new()
            {
                ProductId = proid,
                UserId = userid,
                AddedDate = DateTime.Now
            };

            _context.favoriteProducts.Add(favoriteProduct);
            _context.SaveChanges();
        }

        public bool CreateProduct(Product model)
        {
            _context.products.Add(model);
            return Save();
        }



        public bool DeleteProduct(int proId)
        {
            var product = GetproductById(proId);
            _context.products.Remove(product);
            return Save();
        }

        public bool EditeProduct(Product product)
        {
            _context.Update(product);
            return Save();

        }

        public bool existingFavorite(int userid, int proid)
        {
            _context.favoriteProducts.FirstOrDefault(f => f.UserId == userid && f.ProductId == proid);
            return true;
        }

        public List<Category> GetAllCategories()
        {
            return _context.categories.ToList();
        }

        public List<ProductForIndex> GetAllForIndex()
        {
            List<ProductForIndex> productForIndex = new List<ProductForIndex>();
            var pe = _context.products.ToList();
            foreach (var item in pe)
            {
                var p = new ProductForIndex() { ProductId = item.ProductId, Name = item.Name, Price = item.Price, Imagename = item.imagename };
                productForIndex.Add(p);
            }
            return productForIndex;
        }

        public List<Product> GetAllProduct()
        {
            return _context.products.ToList();
        }

        public List<Product> GetAllProductForAdmin()
        {
            return _context.products.ToList();
        }

        public int GetGroupIdByName(string name)
        {
            return _context.categories.SingleOrDefault(c => c.Name == name).CategoryId;
        }

        public Product GetproductById(int proid)
        {
            return _context.products.Find(proid);
        }

        public int GetProductidByname(string name)
        {

            return _context.products.SingleOrDefault(c => c.Name == name).ProductId;

        }

        public List<Product> GetUserFavorites(int userid)
        {
            bool userExists = _context.users.Any(u => u.UserId == userid);
            if (!userExists)
            {
                throw new Exception("کاربر یافت نشد.");
            }

            // گرفتن لیست علاقه‌مندی‌ها
            var favorites = _context.favoriteProducts
                .Where(fp => fp.UserId == userid)
                .Include(fp => fp.Product) // بارگذاری اطلاعات محصول
                .Select(fp => fp.Product)
                .ToList();

            return favorites;
        }

        public bool IsExistproduct(int proid)
        {
            _context.products.Any(p => p.ProductId == proid);
            return true;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }


    }
}
