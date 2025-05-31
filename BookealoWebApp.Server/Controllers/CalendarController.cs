using Bookealo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Email claim not found.");
            }

            var results = _calendarRepository.GetCalendars(email);
            return Ok(results);
        }

        [HttpPost]
        public IActionResult Save([FromBody] Calendar calendar)
        {
            _calendarRepository.AddCalendar(calendar);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Calendar calendar)
        {
            _calendarRepository.UpdateCalendar(calendar);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Email claim not found.");
            }

            var calendar = _calendarRepository.GetCalendarById(email, id);
            if(calendar == null)
            {
                return NotFound();
            }

            _calendarRepository.RemoveCalendar(calendar);

            return Ok();
        }
    }
}
