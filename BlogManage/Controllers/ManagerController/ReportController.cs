using BlogManage.Services.ManagerServices;
using BlogManage.ViewModel.ManagerVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogManage.Controllers.ManagerController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public ActionResult<ReportVM> GetReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
            }

            var report = _reportService.GenerateReport(startDate, endDate);
            return Ok(report);
        }
    }
}
