using Microsoft.AspNetCore.Mvc;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class ProductBackController : Controller
    {
        private IWebHostEnvironment _enviro;
        private MovieContext movieContext;
        public ProductBackController(IWebHostEnvironment web,MovieContext db)
        {
            movieContext = db;
            _enviro = web;
        }

        public IActionResult Edit()
        {
            int id = 9;
            var prod=movieContext.TProducts.FirstOrDefault(p => p.FProductId == id);
            CTProductWrap productWrap = new CTProductWrap();
            productWrap.product = prod;
            return View(productWrap);
        }
        [HttpPost]
        public IActionResult Edit(CTProductWrap product)
        {
            product.FProductId = 9;
            var prod = movieContext.TProducts.FirstOrDefault(p => p.FProductId == product.FProductId);
            if(prod!=null)
            {
                if(product.photo!=null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/images/products/"+photoName;
                    product.photo.CopyTo(new FileStream(path, FileMode.Create));
                    prod.FImagePath = photoName;
                }
            }

            movieContext.SaveChanges();
            return Content("新增照片成功");
        }
    }
}
