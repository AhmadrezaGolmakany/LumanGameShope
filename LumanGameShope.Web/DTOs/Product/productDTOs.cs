using System.ComponentModel.DataAnnotations;

namespace LumanGameShope.Web.DTOs.Product
{
    public class ProductForIndex
    {
        public int ProductId { get; set; }

        [MaxLength(50, ErrorMessage = "{0}نمیتواند بیتر از {1}  کاراکتر باشد .")]
        public string Name { get; set; }

        [Display(Name = "قیمت محصول")]
        public long Price { get; set; }

        public string Imagename { get; set; }

    }

}
