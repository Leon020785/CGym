using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CGym.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        // GET all activities
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _activityService.GetActivitiesAsync();
            return Ok(activities);
        }

        // GET activity by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var activity = await _activityService.GetByIdAsync(id);

            if (activity == null)
                return NotFound();

            return Ok(activity);
        }

        // POST create activity
        [HttpPost]
        public async Task<IActionResult> Create(Activity activity)
        {
            var created = await _activityService.CreateAsync(activity);
            return Ok(created);
        }

        // PUT update activity
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Activity activity)
        {
            var updated = await _activityService.UpdateAsync(id, activity);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE activity
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _activityService.DeleteAsync(id);
            return NoContent();
        }
    }
}