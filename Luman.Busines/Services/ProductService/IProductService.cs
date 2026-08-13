using Luman.Busines.DTOs.ProductDTO;
using Luman.DataLayer.EntityModel.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luman.Busines.Services.ProductService
{
    public interface IProductService
    {

        #region CategoryAdmin

        bool AddCategory(Category model);

        List<Category> GetAllCategories();

        int GetGroupIdByName(string name);



        #endregion

        #region AdminProduct

        Product GetproductById(int proid);

        bool EditeProduct(Product product);

        public void addgroup(Product product, int groupid);

        bool CreateProduct(Product model);

        List<Product> GetAllProduct();

        List<Product> GetAllProductForAdmin();

        bool DeleteProduct(int proId);

        int GetProductidByname(string name);

        void AddToFavorite(int proid , int userid);

        bool IsExistproduct(int proid);

        bool existingFavorite(int userid , int proid);

        List<Product> GetUserFavorites(int userid);

        bool Save();

        #endregion

        #region product
        List<ProductForIndex> GetAllForIndex();




        #endregion


    }
}
