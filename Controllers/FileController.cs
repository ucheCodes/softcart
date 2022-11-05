using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace dbreezeDbApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    public static IWebHostEnvironment? _webHostEnvironment;
    public FileController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Get()
    {
        return Ok("File Upload Api for peter's soft databases running");
    }
    [HttpPost]
    public IActionResult Upload(IFormFile file)
    {
        try
        {
            if (file.Length > 0)
            {
                Images image = new Images();
		        bool isSaved = false;
                string dir = Directory.GetCurrentDirectory();
                string path = Path.Combine(dir,("Images\\uploads\\"));
                //string path = _webHostEnvironment.WebRootPath+"\\Images\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }             
                string filePath = path+Guid.NewGuid().ToString()+file.FileName;
                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                    isSaved = true;
                }
                if(isSaved == true)
                {
                    byte[] imageByte = System.IO.File.ReadAllBytes(filePath);
                    image.Filepath = filePath;
                    image.FileByte = imageByte;
                    return Ok(image);
                }
                return Ok("web host environment returned null");
            }
            else{return Ok("No File Uploaded");}
        }
        catch (Exception error)
        {
            return Ok(error.Message);
        }
    }
        [HttpDelete("{id}")]
        public JsonResult DeleteAll (string id)
        {
            try
            {
		         String path = id.Replace('_','\\');
                 if(System.IO.File.Exists(path))
                 {
                     System.IO.File.Delete(path);
                     return new JsonResult("Image deleted successfully");
                 }
                 return new JsonResult("Image Delete operation Failed! \n File not Found!");
               // return new JsonResult(path);
            }
            catch (System.Exception error)
            {
                return new JsonResult(error.Message);
            }
        }
}
