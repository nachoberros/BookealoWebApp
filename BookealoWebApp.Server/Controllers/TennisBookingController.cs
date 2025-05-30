using Bookealo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookealoWebApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TennisBookingController : ControllerBase
    {
        private readonly ICourtRepository _courtRepository;
        private readonly ILogger<TennisBookingController> _logger;

        public TennisBookingController(ICourtRepository courtRepository, ILogger<TennisBookingController> logger)
        {
            _courtRepository = courtRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] DateTime? date)
        {
            var results = _courtRepository.Search(date);
            return Ok(results);
        }
    }
}
