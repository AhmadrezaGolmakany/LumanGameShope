using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luman.Busines.DTOs.ProductDTO
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

    public class CreateProductDTO
    {


        [Required]
        [Display(Name = "نام محصول")]
        [MaxLength(50, ErrorMessage = "{0}نمیتواند بیتر از {1}  کاراکتر باشد .")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "قیمت محصول")]
        public long Price { get; set; }

        public IFormFile Imagename { get; set; }

        [Required]
        public string Categoryname { get; set; }
    }

    public class EditeProduct
    {

        [Required]
        [Display(Name = "نام محصول")]
        [MaxLength(50, ErrorMessage = "{0}نمیتواند بیتر از {1}  کاراکتر باشد .")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "قیمت محصول")]
        public long Price { get; set; }

        public IFormFile? Imagename { get; set; }


    }
}
