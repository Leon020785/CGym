using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CGym.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trainers = await _trainerService.GetTrainersAsync();
            return Ok(trainers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTrainerRequest request)
        {
            var trainer = new Trainer { Name = request.Name,
                                        Email = request.Email,
                                        PhoneNumber = request.PhoneNumber,
                                        Availability = request.Availability};
            await _trainerService.CreateTrainerAsync(trainer);
            return Ok(trainer);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTrainerRequest request)
        {
            var trainer = new Trainer { Name = request.Name };

            var updated = await _trainerService.UpdateTrainerAsync(id, trainer);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _trainerService.DeleteTrainerAsync(id);
            return NoContent();
        }

    }

    public record CreateTrainerRequest(string Name, string Email, string PhoneNumber, string Availability);
}