using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Report.API.Dtos;
using Report.API.Services;
using Shared.BaseController;

namespace Report.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : CustomBaseController
    {
        private readonly RabbitMqService _rabbitMqService;

        public ReportController(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost("report")]
        public IActionResult CreateReport([FromBody] ReportRequest request)
        {
            var report = _rabbitMqService.CreateReport(request);

            return Ok(report);
        }

        [HttpGet("report/{uuid}")]
        public IActionResult GetReportStatus(string uuid)
        {
            var reportStatus = _rabbitMqService.GetReportStatus(uuid);

            if (reportStatus == null)
            {
                return NotFound();
            }

            return Ok(reportStatus);
        }

    }
}
