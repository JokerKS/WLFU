using JokerKS.WLFU;
using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Auction;
using JokerKS.WLFU.Entities.Product;
using JokerKS.WLFU.Entities.User;
using JokerKS.WLFU.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JokeKS.WLFU.Controllers
{
    public class AccountController : Controller
    {
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #region Register Get
        public ActionResult Register()
        {
            return View();
        } 
        #endregion

        #region Register Post
        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    FirstName = model.FirstName,
                    Lastname = model.Lastname,
                    BirthDate = model.BirthDate,
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }
        #endregion

        #region Login Get
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        #endregion

        #region Login Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(model.EmailOrUserName, model.Password);
                if (user == null)
                {
                    user = await UserManager.FindByEmailAsync(model.EmailOrUserName);
                }
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid e-mail/username or password.");
                }
                else
                {
                    if (await UserManager.CheckPasswordAsync(user, model.Password))
                    {
                        ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                                DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Home");
                        return Redirect(returnUrl);
                    }
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
        #endregion


        #region Profile Get
        public new ActionResult Profile(string userId = null)
        {
            bool isMyProfile = false;

            // Метода використовується як для відображення власного профілю, так і для відображення чужого профілю
            if(string.IsNullOrEmpty(userId) || userId == User.Identity.GetUserId())
            {
                userId = User.Identity.GetUserId();
                isMyProfile = true;
            }

            var model = new UserProfileModel {
                User = UserManager.FindById(userId)
            };

            model.UserProducts = ProductManager.GetListByDesigner(model.User.Id, true);
            model.UserAuctions = AuctionManager.GetListByDesigner(model.User.Id, true);

            // Беремо зображення до продуктів
            var productImages = model.UserProducts.Where(x => x.MainImageId.HasValue).Select(x => x.MainImageId.Value);
            model.ProductMainImages = ImageManager.GetByIds(productImages).ToDictionary(x => x.Id, v => v);

            var auctionImages = model.UserAuctions.Where(x => x.MainImageId.HasValue).Select(x => x.MainImageId.Value);
            model.AuctionMainImages = ImageManager.GetByIds(auctionImages).ToDictionary(x => x.Id, v => v);

            if (isMyProfile)
            {
                return View("MyProfile", model);
            }
            else
            {
                return View("Profile", model);
            }
        } 
        #endregion
    }
}