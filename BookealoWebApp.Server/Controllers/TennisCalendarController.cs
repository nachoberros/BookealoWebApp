using Bookealo.CommonModel.TennisBooking;
using Bookealo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookealoWebApp.Server.Controllers
{
    [Authorize(Policy = "CanManageCalendar")]
    [ApiController]
    [Route("api/[controller]")]
    public class TennisCalendarController : BookealoBaseController
    {
        private readonly ICourtRepository _courtRepository;
        private readonly ILogger<TennisCalendarController> _logger;

        public TennisCalendarController(ICourtRepository courtRepository, ILogger<TennisCalendarController> logger)
        {
            _courtRepository = courtRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery]int calendarId, DateTime? date)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var results = _courtRepository.Search(AccountId.Value, calendarId, date);
            return Ok(results);
        }

        [HttpPost]
        public IActionResult Save([FromBody] BookingRequest booking)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");
            booking.AccountId = AccountId.Value;

            _courtRepository.Save(booking);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Cancel([FromQuery]int calendarId, int courtId, [FromQuery] DateTime date, [FromQuery] int userId)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var booking = new BookingRequest
            {
                CalendarId = calendarId,
                AccountId = AccountId.Value,
                AssetId = courtId,
                Date = date,
                UserId = userId
            };

            _courtRepository.Cancel(booking);
            return Ok();
        }

        [HttpPut("block")]
        public IActionResult Block([FromBody] BookingRequest booking)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");
            booking.AccountId = AccountId.Value;

            _courtRepository.Block(booking);
            return Ok();
        }

        [HttpPut("unblock")]
        public IActionResult Unblock([FromBody] BookingRequest booking)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");
            booking.AccountId = AccountId.Value;

            _courtRepository.Unblock(booking);
            return Ok();
        }
    }
}
