using CarPartsStore.Models;
using Microsoft.AspNetCore.Mvc;
using CarPartsStore.Repositories.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;

namespace CarPartsStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _CategoryRepo;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryRepository categoryRepo,
                                    ILogger<CategoryController> logger)
        {
            _CategoryRepo = categoryRepo;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                IEnumerable<Category> categories = _CategoryRepo.GetAll();
                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                return View(new Category());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult Add(Category cat)
        {
            try
            {
                _CategoryRepo.Add(cat);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }


        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            try
            {
                Category cat = _CategoryRepo.Get(categoryId);

                return View(cat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult Edit(Category cat)
        {
            try
            {
                _CategoryRepo.Update(cat);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }


        public IActionResult Delete(int catId)
        {
            try
            {
                _CategoryRepo.Delete(catId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
