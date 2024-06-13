using BuyItData.Models;
using BuyItData.Repository.IRepository;
using BuyItUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Drawing.Design;
using System.Security.Claims;

namespace BuyItWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Products.GetAllWithCategoryAsync();
            return View(products);
        }
        public async Task<IActionResult> Details(int? id)
        {
            
            // C2: xóa include ở dòng trên, them var category = await _unitOfWork.Categories.GetByIdAsync(u => u.CategoryID == product.CategoryID);
            //product.Category = category;
            ShoppingCart shoppingCart = new()
            {
                Product = await _unitOfWork.Products.GetByIdAsync(u => u.ProductID == id, includeProperties: "Category"),
                Count = 1,
                ProductID = (int)id
            };
            return View(shoppingCart);
        }
            
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingcart)
        {
            
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingcart.UserID = claim.Value;
            IShoppingCartRepository shoppingCartRepo = (IShoppingCartRepository)_unitOfWork.Carts;
            ShoppingCart cartFromDb = await _unitOfWork.Carts.GetByIdAsync(u => u.UserID == claim.Value && u.ProductID == shoppingcart.ProductID);
            if (cartFromDb == null)
            {
                await _unitOfWork.Carts.AddAsync(shoppingcart);
            }
            else
            {
                await _unitOfWork.Carts.IncrementCount(cartFromDb, shoppingcart.Count);
            }
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
