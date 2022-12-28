using Microsoft.AspNetCore.Mvc;
using Storage.Data.Repositories;

namespace StorageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _config;

        public StorageController(IFileRepository fileRepository, IConfiguration config)
        {
            _fileRepository = fileRepository;
            _config = config;
        }

        [HttpGet]
        public IActionResult GetLogs()
        {
            var logs = _fileRepository.GetLogsFromTmpFile(_config.GetValue<string>("LogFileName"));

            return Ok(logs);
        }
    }
}
