using BlogManage.Models;
using BlogManage.ViewModel.ManagerVM;
using System;
using System.Linq;

namespace BlogManage.Services.ManagerServices
{
    public class ReportServices : IReportService
    {
        private readonly BlogManageContext _context;

        public ReportServices(BlogManageContext context)
        {
            _context = context;
        }

        public ReportVM GenerateReport(DateTime startDateTime, DateTime endDateTime)
        {
            // Chuyển đổi DateTime thành DateOnly
            var startDate = DateOnly.FromDateTime(startDateTime.Date);
            var endDate = DateOnly.FromDateTime(endDateTime.Date);

            var totalBlogs = _context.Blogs.Count(b => b.CreatedDate >= startDate && b.CreatedDate <= endDate);
            var totalComments = _context.Comments.Count(c => c.CreatedDate >= startDate && c.CreatedDate <= endDate);

            var mostAddedCategory = _context.Blogs
                .Where(b => b.CreatedDate >= startDate && b.CreatedDate <= endDate)
                .GroupBy(b => b.CategoryId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.FirstOrDefault().Category.Name)
                .FirstOrDefault();

            var mostBlogsDate = _context.Blogs
                .Where(b => b.CreatedDate >= startDate && b.CreatedDate <= endDate)
                .GroupBy(b => b.CreatedDate)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key.ToString("yyyy-MM-dd"))
                .FirstOrDefault();

            var mostActiveUserId = _context.Comments
                .Where(c => c.CreatedDate >= startDate && c.CreatedDate <= endDate)
                .GroupBy(c => c.UserId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            var mostActiveUser = _context.Profiles
                .Where(u => u.Id == mostActiveUserId)
                .Select(u => u.Name)
                .FirstOrDefault();

            return new ReportVM
            {
                TotalBlogs = totalBlogs,
                TotalComments = totalComments,
                MostAddedCategory = mostAddedCategory ?? "Không có",
                MostBlogsDate = mostBlogsDate ?? "Không có",
                MostActiveUser = mostActiveUser ?? "Không có"
            };
        }
    }
}