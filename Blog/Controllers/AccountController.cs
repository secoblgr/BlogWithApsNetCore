using Blog.Context;
using BlogV1.Identity;
using BlogV1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogV1.Controllers
{
    public class AccountController : Controller
    {
        //db baglantı işlemlerini yapıyoruz.
        private readonly BlogDbContext _context;
        private readonly UserManager<BlogIdentityUser> _userManager;             // user işlemlerini kontrol etmek için usermanager ekledik. 
        private readonly SignInManager<BlogIdentityUser> _signInManager;        // login işlemlerini kontrol için ekledik.

        public AccountController(BlogDbContext context, UserManager<BlogIdentityUser> userManager, SignInManager<BlogIdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // admin veya normal use riçin giriş moetdumuz.
        public IActionResult Login()
        {
          
            return View();
        }


        // login giriş için post işlemi.
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);    // kullanıcı var mı kontrolü. -- email

            if (user == null)
            {
                ViewBag.err = "User Not Found !";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.err = "Email or Password Wrong !";
                return View();
            }
        }
    }
}
