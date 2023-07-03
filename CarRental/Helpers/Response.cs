using Microsoft.AspNetCore.Mvc;

namespace CarRental.Helpers
{
    public class Response : ControllerBase
    {
        public async Task<IActionResult> GetErrorResponse(HttpResponseMessage message)
        {
            var errorMessage = await message.Content.ReadAsStringAsync();
            return StatusCode((int)message.StatusCode, errorMessage);
        }
    }
}
