using AutoMapper;
using BlogManage.Models;
using BlogManage.ViewModel.AdminVM;
using BlogManage.ViewModel.ManagerVM;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogManage.Services.ManagerServices
{
    public class CategoryServices : ICategoryService
    {
        private readonly BlogManageContext _context;
        private readonly IMapper _mapper;
        public CategoryServices(BlogManageContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<CategoryVM> GetCategories()
        {
            var categories = _context.Categories.ToList();
            return _mapper.Map<List<CategoryVM>>(categories);
        }
        public CategoryVM GetCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(p => p.Id == id);
            return category != null ? _mapper.Map<CategoryVM>(category) : null;
        }
        public void CreateCategory(CategoryCreateVM vm)
        {
            vm.Name = vm.Name.Trim();
            if (_context.Categories.Any(p => p.Name == vm.Name))
                throw new Exception("Category đã được sử dụng.");
            var category = new Category
            {
                Name = vm.Name,
                Status = true,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            
        }
        public void UpdateCategory(int id, CategoryCreateVM vm)
        {
            // Trim tên danh mục
            vm.Name = vm.Name.Trim();

            // Kiểm tra xem danh mục có tồn tại không
            var category = _context.Categories.FirstOrDefault(p => p.Id == id);
            if (category == null)
            {
                throw new Exception("Category không tồn tại.");
            }

            // Kiểm tra tên có trùng với danh mục khác không
            if (_context.Categories.Any(p => p.Name == vm.Name && p.Id != id))
            {
                throw new Exception("Tên category đã được sử dụng.");
            }

            // Cập nhật thông tin danh mục
            category.Name = vm.Name;
            category.Status = vm.Status; // Cập nhật trạng thái
            category.UpdateDate = DateOnly.FromDateTime(DateTime.Now);
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            // Kiểm tra xem danh mục có tồn tại không
            var category = _context.Categories.Include(c => c.Blogs).FirstOrDefault(p => p.Id == id);
            if (category == null)
            {
                throw new Exception("Category không tồn tại.");
            }

            // Kiểm tra xem danh mục có liên quan đến blog nào không
            if (category.Blogs.Any())
            {
                throw new Exception("Không thể xóa category vì nó liên quan đến blog.");
            }

            // Xóa danh mục
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
