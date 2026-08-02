using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IUnitOfWork _myUnit;
        public CategoriesController(IUnitOfWork myUnit , ILogger<CategoriesController> logger)
        {
            _myUnit = myUnit;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? search)
        {
            IEnumerable<Category> categories;

            if (string.IsNullOrWhiteSpace(search))
            {
                categories = await _myUnit.categories.GetAllAsync();
            }
            else
            {
                categories = await _myUnit.categories.SearchAsync(search);
            }

            ViewBag.Search = search;

            return View(categories);
        }

        //Get
        public IActionResult New()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var existingCategory = await _myUnit.categories.GetByNameAsync(category.Name);

            if (existingCategory != null)
            {
                ModelState.AddModelError("Name", "Category name already exists.");
                return View(category);
            }

            await _myUnit.categories.AddAsync(category);
            await _myUnit.SaveAsync();

            _logger.LogInformation("Category '{CategoryName}' created successfully.", category.Name);

            TempData["SuccessData"] = "Category has been added successfully.";

            return RedirectToAction(nameof(Index));
        }

        //Get
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = await _myUnit.categories.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            var existingCategory = await _myUnit.categories.GetByNameAsync(category.Name);

            if (existingCategory != null && existingCategory.Id != category.Id)
            {
                ModelState.AddModelError("Name", "Category name already exists.");
                return View(category);
            }

            var categoryFromDb = await _myUnit.categories.GetByIdAsync(category.Id);

            if (categoryFromDb == null)
                return NotFound();

            categoryFromDb.Name = category.Name;
            categoryFromDb.Description = category.Description;

            await _myUnit.SaveAsync();

            TempData["SuccessData"] = "Category updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        //Get
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var category = await _myUnit.categories.GetByIdAsync(Id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Category category)
        {
            _myUnit.categories.Delete(category);
            await _myUnit.SaveAsync();
            TempData["SuccessData"] = "Item has been delete successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = await _myUnit.categories.GetByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

    }
}
