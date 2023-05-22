using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Net;
using System.Security.Policy;

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

            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads", fileName);
            string url = "https://" + this.Request.Host.Value.ToString() + "/images/uploads/" + fileName;

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok(new { success = 1, file = new { url } });
        }
        [HttpPost("fetchURL")]
        public async Task<IActionResult> fetchURL(string imgurl)
        {
            string url = "";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(imgurl);
                if (response.IsSuccessStatusCode)
                {
                    byte[] data = await response.Content.ReadAsByteArrayAsync();
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads");
                    var fileName = Guid.NewGuid().ToString() + ".jpg";
                    var filePath = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileStream.WriteAsync(data, 0, data.Length);
                        url = "https://" + this.Request.Host.Value.ToString() + "/images/uploads/" + fileName;
                        return Ok(new { success = 1, File = new { url } });
                    }
                }
            }
            return BadRequest("Invalid url.");
        }
    }
}
