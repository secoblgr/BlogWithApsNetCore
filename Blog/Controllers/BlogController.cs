using Blog.Context;
using Blog.Models;
using BlogV1.Identity;
using BlogV1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        //db baglantı işlemlerini yapıyoruz.
        private readonly BlogDbContext _context;
      

        public BlogController(BlogDbContext context)            // sınıfın constructorunu çalıştırıp burda ilk olarak db bağlantımızı yapıyoruz.
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //  var blogs = _context.Blogs.ToList();   //bütün  gelen verimizi listeye atadık.
            var blogs = _context.Blogs.Where(x => x.Status == 1).ToList();   //sadece statusu 1 olanları yani aktif olanları getirir.
            return View(blogs);
        }

        public IActionResult Details(int id)
        {
            var blogs = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();          // id ye göre istenilen veri çekildi..
            blogs.ViewCount++;                                                           // blog sayfasına gittiğimizde viewcountu arrtır.
            var comments = _context.Comments.Where(x => x.BlogId == id).ToList();
            ViewBag.Comments = comments.ToList();
            return View(blogs);
        }

        [HttpPost]
        public IActionResult CreateComment(Comment model)   // kaydedeceğim sınıfın modelini parametre olarak verdik.
        {
            _context.Comments.Add(model);              //db deki tabloya verileri ekleedik.
            model.PublishDate = DateTime.Now;           // gönderme zamanını eşitleme
            var blog = _context.Blogs.Where(x => x.Id == model.BlogId).FirstOrDefault();   // comment sayısı için blogu aldık.
            blog.CommentCount++;                      //her yeni yorum eklediğimiz için count sayımızı arttırdık.
            _context.SaveChanges();                    // db üzerine kaydetme..

            return RedirectToAction("Details", new { id = model.BlogId });          // işlemleri yaptıktan sonra index veya detay sayfasna yönlendirsin.
        }


        public IActionResult About()
        {
            return View();
        }


        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateContact(Contact model)
        {
            _context.Contacts.Add(model);
            model.CreatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Support()
        {
            return View();
        }

    }
}


