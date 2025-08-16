using BlogManage.ViewModel.ManagerVM;
using BlogManage.Models;

namespace BlogManage.Services.ManagerServices
{
    public interface IReportService
    {
        ReportVM GenerateReport(DateTime startDateTime, DateTime endDateTime);
    }
}
