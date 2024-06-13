using BuyItData.Models;
using BuyItData.ViewModels;
using BuyItData.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using BuyItUtility;
using Microsoft.AspNetCore.Authorization;
using Stripe.Checkout;

namespace BuyItWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
	[BindProperties]
	public class CartController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;        
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                CartList = await _unitOfWork.Carts.GetAllAsync(u => u.UserID == claim.Value, includeProperties: "Product"),
                OrderHeader =new ()

            };
            foreach (var cart in ShoppingCartVM.CartList)
            {
                cart.PriceReal = GetPriceBaseOnQuantity(cart.Count, cart.Product.ListPrice, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.PriceReal * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        public async Task<IActionResult> Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                CartList = await _unitOfWork.Carts.GetAllAsync(u => u.UserID == claim.Value, includeProperties:"Product"),
                OrderHeader = new ()    

            };
            ShoppingCartVM.OrderHeader.User = await _unitOfWork.Users.GetByIdAsync(u => u.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.User.Name;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.User.StreetAddress;
            ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.User.PhoneNumber;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.User.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.User.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.User.PostalCode;

            foreach (var cart in ShoppingCartVM.CartList)
            {
                cart.PriceReal = GetPriceBaseOnQuantity(cart.Count, cart.Product.ListPrice, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.PriceReal * cart.Count);
            }
            return View(ShoppingCartVM);
		}

		[ActionName("Summary")]
		[HttpPost]
		public async Task<IActionResult> SummaryPOST()
		{
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM.CartList = await _unitOfWork.Carts.GetAllAsync(u => u.UserID == claim, includeProperties:"Product");

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.UserID = claim;

			//ShoppingCartVM = new ShoppingCartVM()
   //         { 
			//	OrderHeader = new()

			//};
			//ShoppingCartVM.OrderHeader.User = await _unitOfWork.Users.GetByIdAsync(u => u.Id == claim.Value);

			//ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.User.Name;
			//ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.User.StreetAddress;
			//ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.User.PhoneNumber;
			//ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.User.City;
			//ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.User.State;
			//ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.User.PostalCode;

			foreach (var cart in ShoppingCartVM.CartList)
			{
				cart.PriceReal = GetPriceBaseOnQuantity(cart.Count, cart.Product.ListPrice, cart.Product.Price50, cart.Product.Price100);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.PriceReal * cart.Count);
			}
            await _unitOfWork.OrderHeaders.AddAsync(ShoppingCartVM.OrderHeader);
			await _unitOfWork.Save();

			foreach (var cart in ShoppingCartVM.CartList)
			{
				OrderDetail orderDetail = new OrderDetail()
                {
                    ProductID = cart.ProductID,
                    OrderHeaderID = ShoppingCartVM.OrderHeader.OrderHeaderID,
                    PriceReal = cart.PriceReal,
                    Count = cart.Count
                };
                await _unitOfWork.OrderDetails.AddAsync(orderDetail);
                await _unitOfWork.Save();
            }

            //Payment settings
            var domain = "https://localhost:44351/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>()
                ,
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.OrderHeaderID}",
                CancelUrl = domain + $"customer/cart/Index",
            };

            foreach (var item in ShoppingCartVM.CartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.PriceReal * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ProductName,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

   //         _unitOfWork.Carts.RemoveRange(ShoppingCartVM.CartList);
			//await _unitOfWork.Save();
			//return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Plus(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(u => u.ShoppingCartID == id);
            await _unitOfWork.Carts.IncrementCount(cart, 1);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Minus(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(u => u.ShoppingCartID == id);
            if (cart.Count <= 1)
            {
                //remove that from cart
                _unitOfWork.Carts.DeleteAsync(cart);

            }
            else
            {
                await _unitOfWork.Carts.DecrementCount(cart, 1);
            }            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(u => u.ShoppingCartID == id);
            await _unitOfWork.Carts.DeleteAsync(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        private double GetPriceBaseOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
            {
                return price;
            } else if (quantity < 100 && quantity >= 50)
            {
                return price50;
            } else return price100;
        }
    }
}
