using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CGym.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityController(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        // GET all activities
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _activityRepository.GetAllAsync();
            return Ok(activities);
        }

        // GET activity by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);

            if (activity == null)
                return NotFound();

            return Ok(activity);
        }

        // POST create activity
        [HttpPost]
        public async Task<IActionResult> Create(Activity activity)
        {
            await _activityRepository.AddAsync(activity);
            return Ok(activity);
        }

        // DELETE activity
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _activityRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}