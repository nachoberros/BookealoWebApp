using Bookealo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Calendar = Bookealo.CommonModel.Calendars.Calendar;

namespace BookealoWebApp.Server.Controllers
{
    [Authorize(Policy = "CanManageCalendar")]
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(ICalendarRepository calendarRepository, ILogger<CalendarController> logger)
        {
            _calendarRepository = calendarRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            var results = _calendarRepository.GetCalendars(accountId);
            return Ok(results);
        }

        [HttpGet("{calendarId}")]
        public IActionResult GetById(int calendarId)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            var result = _calendarRepository.GetCalendarById(accountId, calendarId);
            if (result == null)
            {
                return NotFound($"Calendar with ID {calendarId} not found.");
            }

            return Ok(result);
        }


        [HttpPost]
        public IActionResult Save([FromBody] Calendar calendar)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            _calendarRepository.AddCalendar(accountId, calendar);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Calendar calendar)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            _calendarRepository.UpdateCalendar(accountId, calendar);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            var calendar = _calendarRepository.GetCalendarById(accountId, id);
            if(calendar == null)
            {
                return NotFound();
            }

            _calendarRepository.RemoveCalendar(accountId, calendar);

            return Ok();
        }
    }
}
