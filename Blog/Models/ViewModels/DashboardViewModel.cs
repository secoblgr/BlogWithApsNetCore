
namespace BlogV1.Models.ViewModels
{
    public class DashboardViewModel
    {
        public Blog.Models.Blog MostViewedBlog { get; set; }
        public Blog.Models.Blog LatestBlog { get; set; }    
        public int TotalBlogCount { get; set; }
        public int TotalViewCount { get; set; }
        public int TotalCommentCount { get; set; }
        public Blog.Models.Blog MostCommentBlog { get; set; }
        public int TodayCommentCount { get; set; }
    }
}
