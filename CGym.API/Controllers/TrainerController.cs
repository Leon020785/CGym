using CGym.Application.Interfaces;
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
    }
}
