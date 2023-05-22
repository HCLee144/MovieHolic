using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace prjMovieHolic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploaderController : ControllerBase
    {

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            string fileName = Guid.NewGuid().ToString()+".jpg";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images", "uploads", fileName);
            string url = "https://" +this.Request.Host.Value.ToString()+"/images/uploads/"+fileName;

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok(new {success=1, file=new { url } });
        }
    }
}
