using CGym.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace CGym.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerRepository _trainerRepository;

        public TrainerController(ITrainerRepository trainerRepository)
            => _trainerRepository = trainerRepository;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trainers = await _trainerRepository.GetTrainersAsync();
            return Ok(trainers);
        }
    }
}
