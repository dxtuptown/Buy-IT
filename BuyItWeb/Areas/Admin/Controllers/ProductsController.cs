using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuyItData.Data;
using BuyItData.Models;
using BuyItData.Repository.IRepository;

namespace BuyItWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Products.GetAllWithCategoryAsync();
            return View(products);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            

            var product = await _unitOfWork.Products.GetByIdAsync(u => u.ProductID == id);
           
            
            return View(product);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["CategoryID"] = new SelectList(await _unitOfWork.Categories.GetAllAsync(), "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,Description,ISBN,Author,ListPrice,Price50,Price100,ImageUrl,CategoryID")] Product product, IFormFile? fileImage)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (fileImage != null)
                {

                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(fileImage.FileName);
                    if (product.ImageUrl != null)
                    {
                        var OldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                         fileImage.CopyTo(fileStreams);
                    }
                    if (extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        product.ImageUrl = @"images\products\" + fileName + extension;
                    }
                    else
                    {
                        ViewData["imageError"] = "Invalid image file format. Only .jpg and .png files are supported.";
                    }

                }
                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.Save();
                TempData["Success"] = "Created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(await _unitOfWork.Categories.GetAllAsync(), "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _unitOfWork.Products == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.Products.GetByIdAsync(u => u.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(await _unitOfWork.Categories.GetAllAsync(), "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,Description,ISBN,Author,ListPrice,Price50,Price100,ImageUrl,CategoryID")] Product product, IFormFile? fileImage)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (fileImage != null)
                    {
                        
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"images\products");
                        var extension = Path.GetExtension(fileImage.FileName);
                        if (product.ImageUrl != null)
                        {
                            var OldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.Trim('\\'));
                            if (System.IO.File.Exists(OldImagePath))
                            {
                                System.IO.File.Delete(OldImagePath);
                            }
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            fileImage.CopyTo(fileStreams);
                        }
                        if (extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                        {
                            product.ImageUrl = @"images\products\" + fileName + extension;
                        }
                        else
                        {
                            ViewData["imageError"] = "Invalid image file format. Only .jpg and .png files are supported.";
                        }
                        
                    }
                    await _unitOfWork.Products.UpdateAsync(product);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Success"] = "Edited successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(await _unitOfWork.Categories.GetAllAsync(), "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitOfWork.Products == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.Products.GetByIdAsync(u => u.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(await _unitOfWork.Categories.GetAllAsync(), "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.Products == null)
            {
                return Problem("Entity set 'MyDbContext.Products'  is null.");
            }
            var product = await _unitOfWork.Products.GetByIdAsync(u => u.ProductID == id);
            if (product != null)
            {
                await _unitOfWork.Products.DeleteAsync(product);
            }
            TempData["Success"] = "Deleted successfully!";
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_unitOfWork.Products.GetByIdAsync(u => u.ProductID == id) != null);
        }

        public string UploadImage(IFormFile file, string directory, string? oldImageUrl = "")
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString();

            var uploadDirectory = Path.Combine(wwwRootPath, directory);

            var extension = Path.GetExtension(file.FileName);

            if (!string.IsNullOrEmpty(oldImageUrl))
            {
				var oldImagePath = Path.Combine(wwwRootPath, oldImageUrl.TrimStart('\\'));
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}
			}
			// Open stream to upload file
			using (var fileStreams = new FileStream(Path.Combine(uploadDirectory, fileName + extension), FileMode.Create))
			{
				file.CopyTo(fileStreams);
			}
			return $@"\{directory}\{fileName}{extension}";
		}

        #region API_CALLS
        [HttpGet]
        public  IActionResult GetAll()   
        {
            var productList =  _unitOfWork.Products.GetAllAsync(includeProperties: "Category");
            return  Json(new { data = productList });
        }
        #endregion
    }
}
