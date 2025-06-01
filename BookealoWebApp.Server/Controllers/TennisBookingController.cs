using Bookealo.CommonModel.Account;
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
        public IActionResult GetAll([FromQuery]int calendarId, DateTime? date)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            var results = _courtRepository.Search(accountId, calendarId, date);
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
                AssetId = courtId,
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
