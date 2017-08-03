using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlarinWood.Services;
using GlarinWood.Data;
using GlarinWood.Models.CheckoutViewModel;
using Microsoft.EntityFrameworkCore;
using GlarinWood.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Net.Http;
using GlarinWood.Utilites;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GlarinWood.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailForForgetPassword _EmailPassword;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //private readonly IHostingEnvironment _environment;
        #region ctr
        public ShoppingCartController(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpcontext, ApplicationDbContext context
            , UserManager<ApplicationUser> userManager, IEmailForForgetPassword EmailPassword)
        {
            _signInManager = signInManager;
            _context = context;
            _contextAccessor = httpcontext;
            _userManager = userManager;
            _EmailPassword = EmailPassword;
        }
        #endregion
        #region Checkout
        // GET: Checkout
        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult Checkout()
        {

            return View();
        }

        // POST: Checkout
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return (View(model));
            }

            //review
            var cart = new ShoppingCart(_contextAccessor, _context);
            //ایمیل کاربر را می گیریم ، اگر ثبت نام نکرده بود، ثبت نام صورت می گیرد
            //بعد از ثبت نام ، رمز عبور به کاربر ایمیل می شود و کاربر را به صفحه ی تغییر گذرواژه هدایت میکنیم.

            model.UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            var result = cart.CheckOut(model);

            return RedirectToAction("Pay");
        }
        #endregion
        #region Pay
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Pay()
        {
            var Cart = new ShoppingCart(_contextAccessor, _context);

            var Model = _context.CartItems.Include(c => c.Product).Where(current => current.CartId == Cart.GetCart(_contextAccessor));
            decimal SumCountPrice = 0;


            if (Model != null)
            {


                foreach (var item in Model)
                {


                    //<!--جهت برآورد جمع کل نهایی-->

                    SumCountPrice = (item.Count * item.Product.Price) + SumCountPrice;

                }
            }
            ViewData["SumCountPrice"] = (int)SumCountPrice;
            return View();
        }

        //پرداخت
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Pay(int price)
        {



            try
            {
                //ایجاد یک شی از جدول ثبت اطلاعات فرآیند پرداخت
                Payment payment = new Payment();

                payment.Amount = price;
                payment.ReferenceNumber = 0;
                // شماره آی دی جدول کاربر  
                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                payment.UserId = userId;

                _context.Add(payment);
                await _context.SaveChangesAsync();

                int paymentId = payment.PaymentId;

                WinHttpHandler httpHandler = new WinHttpHandler();
                httpHandler.MaxConnectionsPerServer.Equals(int.MaxValue);
                httpHandler.SslProtocols = SslProtocols.Tls12;

                // آدرس برگشت از درگاه
                string redirectPage = "http://localhost:11262/ShoppingCart/Return?saleOrderId=" + paymentId.ToString();
                // ایجاد یک شی از وب سرویس اتصال به درگاه زرین پال 
                var zp = new ZarinpalServiceReference.PaymentGatewayImplementationServicePortTypeClient();
                // نکته:» مبلغ به صورت تومان به درگاه زرین پال ارسال می شود
                string authority;
                string merchantCode = "501a433c-6d2f-11e7-b90f-033c295111fc";


                //  ارسال اطلاعات به درگاه
                var paymentReques = zp.PaymentRequestAsync(merchantCode, price / 10, "www.Apostrophes.ir خرید از سایت", "Admin@Info.com", "09010487144", redirectPage);

                // بررسی وضعیت
                if (paymentReques.Result.Body.Status == 100)
                {
                    authority = paymentReques.Result.Body.Authority;

                    // اتصال به درگاه
                    Response.Redirect("https://www.zarinpal.com/pg/StartPay/" + authority);
                }
                else
                {
                    // ویرایش اطلاعات ارسالی از زرین پال در صورت عدم اتصال
                    Payment editPayment = _context.Payment.Find(paymentId);

                    if (editPayment != null)
                    {
                        // ویرایش خطای عدم اتصال به درگاه
                        editPayment.StatusPayment = Convert.ToString(paymentReques);

                        _context.Update(editPayment);
                        await _context.SaveChangesAsync();
                    }

                    // نمایش خطا به کاربر
                    ViewData["Message"] = ZarinPalResult.Status(paymentReques.Result.Body.Status.ToString());
                    ViewData["SumCountPrice"] = price;
                }



            }

            catch (Exception ex)
            {
                string error = ex.Message;
                ViewData["Message"] = "در حال حاضر امکان اتصال به این درگاه وجود ندارد";
                ViewData["SumCountPrice"] = price;

            }
            return View();
        }
        #endregion
        #region ViewCart
        // GET: ShoppingCart
        [HttpGet]
        public IActionResult ViewCart()
        {



            return View();

        }

        public IActionResult _ViewCartPartial()
        {
            var Cart = new ShoppingCart(_contextAccessor, _context);

            //var applicationDbContext = Cart.GetCartItems().ToList();
            var CartItemModels = _context.CartItems.Include(c => c.Product).Where(current => current.CartId == Cart.GetCart(_contextAccessor));


            return PartialView(CartItemModels.ToList());

        }
        #endregion
        #region متد حذف و اضافه  ViewComponent("Basket")
        //Add
        [HttpPost]

        public ActionResult AddToCart(int id, int quantity)
        {
            var Cart = new ShoppingCart(_contextAccessor, _context);
            Cart.Add(id, quantity);
            //return new NoContentResult();
            return ViewComponent("Basket");

        }

        // Remove  return ViewComponent("Basket")
        [HttpGet]
        public ActionResult RemoveFromBasket(int productId)
        {
            var Cart = new ShoppingCart(_contextAccessor, _context);
            Cart.Remove(productId);
            //return PartialView("_BasketPartial");
            return ViewComponent("Basket");
        }
        #endregion
        #region متد حذف و اضافه _ViewCartPartial 

        // AddOne Return _ViewCartPartial
        [HttpPost]

        public IActionResult AddOne(int productId)
        {
            var Cart = new ShoppingCart(_contextAccessor, _context);
            Cart.AddOne(productId);

            //var Cart = new ShoppingCart(_contextAccessor, _context);

            //var applicationDbContext = Cart.GetCartItems().ToList();
            //var applicationDbContext = _context.CartItems.Include(c => c.Product).Where(current => current.CartId == Cart.GetCart(_contextAccessor)).ToList();

            return RedirectToAction("_ViewCartPartial");
        }

        // Remove One Return _ViewCartPartial
        [HttpPost]

        public IActionResult RemoveOne(int productId)
        {
            var Cart = new ShoppingCart(_contextAccessor, _context);
            Cart.Remove(productId);

            //var Cart = new ShoppingCart(_contextAccessor, _context);

            //var applicationDbContext = Cart.GetCartItems().ToList();
            //var applicationDbContext = _context.CartItems.Include(c => c.Product).Where(current => current.CartId == Cart.GetCart(_contextAccessor)).ToList();

            return RedirectToAction("_ViewCartPartial");
        }
        // Remove Return _ViewCartPartial
        [HttpPost]

        public IActionResult RemoveViewCart(int productId)
        {
            var Cart = new ShoppingCart(_contextAccessor, _context);
            //int count= Cart.Remove(productId);
            var product = _context.CartItems.Include(c => c.Product).Where(current => current.CartId == Cart.GetCart(_contextAccessor))
               .Where(p => p.ProductId == productId).FirstOrDefault();
            _context.CartItems.Remove(product);
            _context.SaveChanges();

            //var Cart = new ShoppingCart(_contextAccessor, _context);

            //var applicationDbContext = Cart.GetCartItems().ToList();
            //var applicationDbContext = _context.CartItems.Include(c => c.Product).Where(current => current.CartId == Cart.GetCart(_contextAccessor)).ToList();

            return RedirectToAction("_ViewCartPartial");
        }
        [Authorize(Roles = "User")]
        #endregion
        #region اکشن برگشت از درگاه

        public ActionResult Return(int saleOrderId)

        {
            try
            {
                //حتی می توان ورودی های برگشتی از وب سرویس را بصورت ویو مدل دریافت کرد، یا از طریق ورودی مانند بالا، و یا 
                //همانند پایین httpcobtext
                string saleOrderIdQuery = _contextAccessor.HttpContext.Request.Query["saleOrderId"]; //getting null

                string StatusQuery = _contextAccessor.HttpContext.Request.Query["Status"]; //getting null
                string AuthorityQuery = _contextAccessor.HttpContext.Request.Query["Authority"]; //getting null

                if (saleOrderIdQuery != "" && StatusQuery != "" && StatusQuery != null && AuthorityQuery != "" && AuthorityQuery != null)
                {
                    // بررسی وضعیت پرداخت
                    if (StatusQuery.ToString().Equals("OK"))
                    {
                        int paymentId = Convert.ToInt32(saleOrderId);

                        // پیدا کردن مبلغ پرداختی از دیتابیس 
                        int amount = FindAmountPayment(paymentId);

                        // شماره پیگیری
                        long refId = 0;
                        //System.Net.ServicePointManager.Expect100Continue = false;
                        WinHttpHandler httpHandler = new WinHttpHandler();
                        httpHandler.MaxConnectionsPerServer.Equals(int.MaxValue);
                        httpHandler.SslProtocols = SslProtocols.Tls12;


                        // ایجاد یک شی از وب سرویس اتصال به درگاه زرین پال 
                        var zp = new ZarinpalServiceReference.PaymentGatewayImplementationServicePortTypeClient();

                        // کد پذیرنده
                        string merchantCode = "501a433c-6d2f-11e7-b90f-033c295111fc";
                        // وری فای کردن اطلاعات

                        var paymentVerification = zp.PaymentVerificationAsync(merchantCode, AuthorityQuery, amount);

                        // بررسی وضعیت تایید پرداخت
                        if (paymentVerification.Result.Body.Status == 100)
                        {
                            refId = paymentVerification.Result.Body.RefID;

                            // پرداخت موفق بوده و اطلاعات دریافتی را در دیتابیس ثبت می کنیم
                            // در این قسمت می توانید اطلاعات دریافتی را در دیتابیس ذخیره کنید
                            Payment payment = _context.Payment.Find(paymentId);

                            if (payment != null)
                            {
                                payment.ReferenceNumber = refId;
                                payment.SaleReferenceId = AuthorityQuery.ToString();
                                payment.StatusPayment = Convert.ToString(paymentVerification.Result.Body.Status); 
                                _context.Update(payment);
                                //بعد از ثبت موفقیت آمیز فرآیند پرداخت
                                //سبد خرید را خالی کرده
                                var Cart = new ShoppingCart(_contextAccessor, _context);
                                var items = _context.CartItems.Include(c => c.Product).Where(current => current.CartId == Cart.GetCart(_contextAccessor));
                                // clear ShoppingCart
                                _context.CartItems.RemoveRange(items); 
                               // شماره آی دی جدول کاربر  
                                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                                //آخرین سفارش کاربر را گرفته و مهر تائید بر آن می زنیم
                                Order order = _context.Order.Where(c => c.UserId == userId).OrderByDescending(d => d.OrderDate).FirstOrDefault();
                                //مهر تائید
                                order.IsBuy = true;
                                _context.Update(order); 
                                _context.SaveChanges(); 
                            }
                            ViewBag.SaleID = saleOrderId;
                            ViewBag.PaymentID = paymentId;
                            ViewBag.Message = ZarinPalResult.Status(paymentVerification.Result.Body.Status.ToString());
                            ViewBag.SaleReferenceId = refId;
                            //ViewBag.Image = "~/Content/Images/accept.png";


                        }

                        else
                        {

                             Payment payment = _context.Payment.Find(paymentId);

                            if (payment != null)
                            {
                                payment.ReferenceNumber = 0;
                                payment.SaleReferenceId = AuthorityQuery.ToString();
                                payment.StatusPayment = Convert.ToString(paymentVerification.Result.Body.Status);

                                _context.Update(payment);
                               
                                _context.SaveChanges();

                            }
                            ViewBag.SaleID = saleOrderId;
                            ViewBag.PaymentID = paymentId;
                            ViewBag.Message = ZarinPalResult.Status(Convert.ToString(paymentVerification.Result.Body.Status));
                            ViewBag.SaleReferenceId = "**************";
                            //ViewBag.Image = "~/Content/Images/notaccept.png";
                        }

                    }
                    else
                    {
                        int paymentId = Convert.ToInt32(saleOrderId); 
                         Payment payment = _context.Payment.Find(paymentId);

                        if (payment != null)
                        {
                            payment.ReferenceNumber = 0;
                            payment.SaleReferenceId = AuthorityQuery.ToString();
                            payment.StatusPayment = StatusQuery.ToString();
                            _context.Update(payment);
                            
                            _context.SaveChanges();
                        }
                        ViewBag.PaymentID = paymentId;

                        ViewBag.Message = StatusQuery.ToString();
                        ViewBag.SaleReferenceId = "**************";
                        //ViewBag.Image = "~/Content/Images/notaccept.png";
                    }
                }
                else
                {
                    ViewBag.Message = "دسترسی امکانپذیر نیست";
                }
            }
            catch
            {
                ViewBag.Message = "پاسخی دریافت نشد";
            }
            return View();
        }
        #endregion 
        #region پیدا کردن مبلغ خرید
        private int FindAmountPayment(long paymentId)
        {
            int amount = (from p in _context.Payment
                          where p.PaymentId == paymentId
                          select p.Amount).FirstOrDefault();
            return amount;
        }
        #endregion



    }
}