using Business.Abstract;
using Core.Helper;
using Entities.Concreate;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class PictureController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPictureService _pictureService;

        public PictureController(IWebHostEnvironment webHostEnvironment, IPictureService pictureService)
        {
            _webHostEnvironment = webHostEnvironment;
            _pictureService = pictureService;
        }

        public async Task<JsonResult> UploadPicture(List<IFormFile> PhotoUrls)
        {
            return Json( await PhotoUrls.SaveFileRangeAsync(_webHostEnvironment.WebRootPath));
        }


        [HttpPost]
        public async Task<JsonResult> RemovePicture(string PhotoUrl)
        {

            var result =  await  _pictureService.RemoveProductPictureAsync(PhotoUrl);
            if (result.Success)
            {
                return Json("");
            }                  
            var fileName = Path.GetFileName(PhotoUrl);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads\\", fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return Json("");
        }
    }
}
