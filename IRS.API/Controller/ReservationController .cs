using IRS.Application.Interfaces;
using IRS.Application.Services;
using IRS.Domain.Enums;
using IRS.Domain.response;
using Microsoft.AspNetCore.Mvc;

namespace IRS.API.Controller
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpGet("sample-id")]
        public IActionResult GetSampleId()
        {
            return Ok(Guid.NewGuid());
        }

        [HttpPost("{itemId}")]
        public IActionResult Reserve(Guid itemId)
        {
            var response = _service.Reserve(itemId);

            return Ok(new
            {
                reservationId = response.ReservationId,
                result = response.Result.ToString() 
            });
        }

        [HttpPost("{reservationId}/confirm")]
        public IActionResult Confirm(Guid reservationId)
        {
            var result = _service.Confirm(reservationId);

            return result switch
            {
                ReservationResult.Success => Ok(new { message = "Reservation confirmed" }),

                ReservationResult.NotFound => NotFound(new
                {
                    error = "Reservation not found"
                }),

                ReservationResult.Expired => StatusCode(410, new
                {
                    error = "Reservation expired"
                }),

                ReservationResult.AlreadyConfirmed => Conflict(new
                {
                    error = "Already confirmed"
                }),

                _ => StatusCode(500, new { error = "Unexpected error" })
            };
        }

    }
}
