using Blog.Context;
using Blog.Models;
using BlogV1.Identity;
using BlogV1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize]      // login olunmadıgı süreçe buraya erişim olmaycak.
    public class AdminController : Controller
    {

        private readonly BlogDbContext _context;
        private readonly UserManager<BlogIdentityUser> _userManager;
        private readonly SignInManager<BlogIdentityUser> _signInManager;

        public AdminController(BlogDbContext context, UserManager<BlogIdentityUser> userManager, SignInManager<BlogIdentityUser> signInManager)  // ctor oluşruduk. ve servisleri çalıştırdık
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var dashboard = new DashboardViewModel();            //dashbord sayfası için oluştudugumuz viewmodeli çagırdık.
            var today = DateTime.Today;

            var totalBlogCount = _context.Blogs.Count();
            var totalViewedCount = _context.Blogs.Select(x => x.ViewCount).Sum();
            var mostViewedBlog = _context.Blogs.OrderByDescending(x => x.ViewCount).FirstOrDefault();
            var latestBlog = _context.Blogs.OrderByDescending(x => x.PublishDate).FirstOrDefault();
            var totalCommentCount = _context.Comments.Count();
            var mostCommentBlogId = _context.Comments.GroupBy(x => x.BlogId).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();
            var mostCommentBlog = _context.Blogs.Where(x => x.Id == mostCommentBlogId).FirstOrDefault();
            var todayCommentCount = _context.Comments.Where(x => x.PublishDate >= today && x.PublishDate < today.AddDays(1)).Count();

            dashboard.TotalBlogCount = totalBlogCount;
            dashboard.TotalViewCount = totalViewedCount;
            dashboard.MostViewedBlog = mostViewedBlog!;
            dashboard.LatestBlog = latestBlog!;
            dashboard.TotalCommentCount = totalCommentCount;
            dashboard.MostCommentBlog = mostCommentBlog!;
            dashboard.TodayCommentCount = todayCommentCount;

            return View(dashboard);
        }

        public IActionResult Blogs()
        {
            var blogs = _context.Blogs.ToList();                                     // bütün blogları çektik.

            return View(blogs);
        }

        public IActionResult EditBlog(int id)
        {
            var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            return View(blog);
        }

        public IActionResult Delete(int id)                                         //silme işlmei
        {
            var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("Blogs");
        }


        [HttpPost]
        public IActionResult Update(Blog.Models.Blog model)                         // update işlemleri.
        {
            var blog = _context.Blogs.Where(x => x.Id == model.Id).FirstOrDefault();
            blog.Name = model.Name;
            blog.Description = model.Description;
            blog.Tags = model.Tags;
            blog.ImageUrl = model.ImageUrl;

            _context.SaveChanges();
            return RedirectToAction("Blogs");
        }

        public IActionResult ToggleStatus(int id)                                   //status değiştirme işlemleri.
        {
            var blog = _context.Blogs.FirstOrDefault(x => x.Id == id);
            if (blog != null)
            {
                blog.Status = blog.Status == 1 ? 0 : 1;
                _context.SaveChanges();
            }

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }
            return RedirectToAction("EditBlog", "Admin");
        }

        public IActionResult CreateBlog()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBlog(Blog.Models.Blog model)       //blog oluşturma 
        {
            model.PublishDate = DateTime.Now;
            model.Status = 1;
            _context.Blogs.Add(model);

            _context.SaveChanges();
            return RedirectToAction("Blogs", "Admin");   // admin/blogs viewına yönlendirme yapar.

        }

        public IActionResult Comments(int? blogId)
        {
            var comments = new List<Comment>();

            if (blogId == null)
            {
                comments = _context.Comments.ToList();

            }
            else
            {
                comments = _context.Comments.Where(x => x.BlogId == blogId).ToList();
            }
            return View(comments);
        }


        public IActionResult DeleteComment(int id)                       //comment silme işlemi.
        {
            var comments = _context.Comments.Where(x => x.Id == id).FirstOrDefault();
            _context.Comments.Remove(comments);
            _context.SaveChanges();
            return RedirectToAction("Comments", "Admin");                //comments sayfasına dön.

        }



        // kullanıcı kayıt işlemleri.

        public IActionResult Register()   // register view yönlendirme.
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)     // register sayfamızdaki formdaki verileir almak için post actionumuzu oluşturduk.
        {
            if (model.Password == model.RePassword)             // şifreler aynı ise devam et.
            {
                var user = new BlogIdentityUser                // user modelimize göre kullanıcı oluşturma. ve gelen verileri ekleme.
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    UserName = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.err = result.Errors.Select(e => e.Description).ToList();
                    return View();
                }
            }
            else
            {
                ViewBag.msg = "Passwords do not match!";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();                // signinmanager ile çıkış yaptık.
            return RedirectToAction("Index", "Blog");
        }


        public IActionResult Contact()
        {
            var contact = _context.Contacts.ToList();        //bütün mesajlara ulaştık.
            return View(contact);
        }

        public async Task<IActionResult> SearchBlog(string searchText)          // blog sayfasında arama yaparak istenilen blogu bulmamıza yarar.
        {
            var result = await _context.Blogs
                .Where(x => x.Name.Contains(searchText))
                .ToListAsync();
            return View(result); // ✔️ View'e veri gönderiyorsun
        }
    }
}
