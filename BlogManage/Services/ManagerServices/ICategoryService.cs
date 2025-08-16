using BlogManage.ViewModel.AdminVM;
using BlogManage.ViewModel.ManagerVM;

namespace BlogManage.Services.ManagerServices
{
    public interface ICategoryService
    {
        List<CategoryVM> GetCategories();
        CategoryVM GetCategory(int id);
        void CreateCategory(CategoryCreateVM vm);
        void UpdateCategory(int id, CategoryCreateVM vm);
        void DeleteCategory(int id);
    }
}