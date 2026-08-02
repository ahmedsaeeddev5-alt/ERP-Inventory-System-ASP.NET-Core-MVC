using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUnitOfWork _myUnit;
        public ProductsController(IUnitOfWork myUnit, ILogger<ProductsController> logger)
        {
            _myUnit = myUnit;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? search)
        {
            IEnumerable<Product> products;

            if (string.IsNullOrWhiteSpace(search))
            {
                products = await _myUnit.products.GetAllWithDetailsAsync();
            }
            else
            {
                products = await _myUnit.products.SearchAsync(search);
            }

            ViewBag.Search = search;

            return View(products);
        }

        //Get
        public async Task<IActionResult> New()
        {
            await CreateSelectList();
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Product product)
        {
            if (!ModelState.IsValid)
            {

                await CreateSelectList(product.CategoryId, product.UnitId);
                return View(product); 

            }

            var existingProduct = await _myUnit.products.GetByNameAsync(product.Name);

            if (existingProduct != null)
            {
                ModelState.AddModelError("Name", "Product name already exists.");
                await CreateSelectList(product.CategoryId, product.UnitId);
                return View(product);
            }

            if (product.clientfile != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    await product.clientfile.CopyToAsync(stream);
                    product.dbImage = stream.ToArray();
                }
            }

            await _myUnit.products.AddAsync(product);
            await _myUnit.SaveAsync();

            _logger.LogInformation("Product '{ProductName}' created successfully.", product.Name);

            TempData["SuccessData"] = "Product has been added successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task CreateSelectList(int selectedCategoryId = 0, int selectedUnitId = 0)
        {
            var categories = (await _myUnit.categories.GetAllAsync()).ToList();
            var units = (await _myUnit.units.GetAllAsync()).ToList();

            ViewBag.CategoryList = new SelectList(
                categories,
                "Id",
                "Name",
                selectedCategoryId);

            ViewBag.UnitList = new SelectList(
                units,
                "Id",
                "Name",
                selectedUnitId);
        }

        //Get
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = await _myUnit.products.GetByIdWithDetailsAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            await CreateSelectList(product.CategoryId, product.UnitId);
            return View(product);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                await CreateSelectList(product.CategoryId, product.UnitId);
                return View(product);
            }

            var existingProduct = await _myUnit.products.GetByNameAsync(product.Name);

            if (existingProduct != null && existingProduct.Id != product.Id)
            {
                ModelState.AddModelError("Name", "Product name already exists.");
                await CreateSelectList(product.CategoryId, product.UnitId);
                return View(product);
            }

            var productFromDb = await _myUnit.products.GetByIdWithDetailsAsync(product.Id);

            if (productFromDb == null)
                return NotFound();

            if (product.clientfile != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    await product.clientfile.CopyToAsync(stream);
                    productFromDb.dbImage = stream.ToArray();
                }
            }

            productFromDb.Name = product.Name;
            productFromDb.Code = product.Code;
            productFromDb.Barcode = product.Barcode;
            productFromDb.PurchasePrice = product.PurchasePrice;
            productFromDb.SalePrice = product.SalePrice;
            productFromDb.IsActive = product.IsActive;
            productFromDb.CategoryId = product.CategoryId;
            productFromDb.UnitId = product.UnitId;
           

            await _myUnit.SaveAsync();

            TempData["SuccessData"] = "Product updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        //Get
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var product = await _myUnit.products.GetByIdWithDetailsAsync(Id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Product product)
        {
            var productFromDb = await _myUnit.products.GetByIdAsync(product.Id);

            if (productFromDb == null)
                return NotFound();

            _myUnit.products.Delete(productFromDb);
            await _myUnit.SaveAsync();
            TempData["SuccessData"] = "Product has been delete successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = await _myUnit.products.GetByIdWithDetailsAsync(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
