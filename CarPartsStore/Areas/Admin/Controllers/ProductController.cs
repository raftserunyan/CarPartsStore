using Microsoft.AspNetCore.Mvc;
using CarPartsStore.Models;
using CarPartsStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;

namespace CarPartsStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _ProductRepo;
        private readonly ICategoryRepository _CategoryRepo;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepo,
                                ICategoryRepository categoryRepo,
                                ILogger<ProductController> logger)
        {
            _ProductRepo = productRepo;
            _CategoryRepo = categoryRepo;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product prod)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(prod.Description))
                        prod.Description = "No Description...";

                    if (string.IsNullOrWhiteSpace(prod.ImageUrl))
                        prod.ImageUrl = "~/img/noimage.png";

                    prod.Category = _CategoryRepo.Get(prod.CategoryId);

                    _ProductRepo.Update(prod);
                    return Redirect("/Admin/Home/Index#portfolio");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Add(Product prod)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(prod.Description))
                        prod.Description = "No Description...";

                    if (string.IsNullOrWhiteSpace(prod.ImageUrl))
                        prod.ImageUrl = "~/img/noimage.png";

                    prod.Category = _CategoryRepo.Get(prod.CategoryId);

                    _ProductRepo.Add(prod);
                    return Redirect("/Admin/Home/Index#portfolio");
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            
        }

        public IActionResult Delete(int prodId)
        {
            try
            {
                _ProductRepo.Delete(prodId);

                return Redirect("/Admin/Home/Index#portfolio");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
