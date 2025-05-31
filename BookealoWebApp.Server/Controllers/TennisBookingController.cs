using Bookealo.CommonModel.TennisBooking;
using Bookealo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookealoWebApp.Server.Controllers
{
    [Authorize]
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

        [HttpPost]
        public IActionResult Save([FromBody] BookingRequest booking)
        {
            _courtRepository.Save(booking);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Cancel([FromQuery] int courtId, [FromQuery] DateTime date, [FromQuery] int userId)
        {
            var booking = new BookingRequest
            {
                CourtId = courtId,
                Date = date,
                UserId = userId
            };

            _courtRepository.Cancel(booking);
            return Ok();
        }

        [Authorize(Policy = "CanManageAsset")]
        [HttpPut("block")]
        public IActionResult Block([FromBody] BookingRequest booking)
        {
            _courtRepository.Block(booking);
            return Ok();
        }

        [Authorize(Policy = "CanManageAsset")]
        [HttpPut("unblock")]
        public IActionResult Unblock([FromBody] BookingRequest booking)
        {
            _courtRepository.Unblock(booking);
            return Ok();
        }
    }
}
