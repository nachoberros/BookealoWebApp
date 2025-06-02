using Bookealo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Calendar = Bookealo.CommonModel.Calendars.Calendar;

namespace BookealoWebApp.Server.Controllers
{
    [Authorize(Policy = "CanManageCalendar")]
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : BookealoBaseController
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
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var results = _calendarRepository.GetCalendars(AccountId.Value);
            return Ok(results);
        }

        [HttpGet("{calendarId}")]
        public IActionResult GetById(int calendarId)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var result = _calendarRepository.GetCalendarById(AccountId.Value, calendarId);
            if (result == null)
            {
                return NotFound($"Calendar with ID {calendarId} not found.");
            }

            return Ok(result);
        }


        [HttpPost]
        public IActionResult Save([FromBody] Calendar calendar)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            _calendarRepository.AddCalendar(AccountId.Value, calendar);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Calendar calendar)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            _calendarRepository.UpdateCalendar(AccountId.Value, calendar);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var calendar = _calendarRepository.GetCalendarById(AccountId.Value, id);
            if(calendar == null)
            {
                return NotFound();
            }

            _calendarRepository.RemoveCalendar(AccountId.Value, calendar);

            return Ok();
        }
    }
}
