using BlogManage.Services.ManagerServices;
using BlogManage.ViewModel.AdminVM;
using BlogManage.ViewModel.ManagerVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogManage.Controllers.ManagerController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryManageController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryManageController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Lấy danh sách danh mục
        [HttpGet("AllCategory")]
        [Authorize(Roles = "Manager")]
        public ActionResult<List<CategoryVM>> GetCategories()
        {
            var categories = _categoryService.GetCategories();
            return Ok(categories);
        }

        // Lấy thông tin một danh mục theo ID
        [HttpGet("detail/{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult<CategoryVM> GetCategory(int id)
        {
            var category = _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound("Category không tồn tại.");
            }
            return Ok(category);
        }

        // Tạo danh mục mới
        [HttpPost("create")]
        [Authorize(Roles = "Manager")]
        public ActionResult CreateCategory([FromBody] CategoryCreateVM vm)
        {
            try
            {
                _categoryService.CreateCategory(vm);
                return CreatedAtAction(nameof(GetCategory), new { id = vm.Name }, vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cập nhật danh mục
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult UpdateCategory(int id, [FromBody] CategoryCreateVM vm)
        {
            try
            {
                _categoryService.UpdateCategory(id, vm);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Xóa danh mục
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                _categoryService.DeleteCategory(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}