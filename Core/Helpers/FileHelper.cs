using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public static class FileHelper
    {
        //public static async Task<string> SaveFileAsync(this IFormFile file, string WebRootPath)
        //{
        //    var filePath = Path.Combine(WebRootPath, "uploads").ToLower();
        //    if (!Directory.Exists(filePath))
        //    {
        //        Directory.CreateDirectory(filePath);
        //    }
        //    var path = "/uploads/" + Guid.NewGuid().ToString() + file.FileName;
        //    using FileStream fileStream = new(WebRootPath + path, FileMode.Create);
        //    await file.CopyToAsync(fileStream);
        //    return path;
        //}

        public static async Task<string> SaveFileAsync(this IFormFile file, string WeebRootPath)
        {          
            var path = "/uploads/" + Guid.NewGuid() + file.FileName;
            using FileStream fileStream = new(WeebRootPath + path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return path;
        }


        public static async Task<List<string>> SaveFileRangeAsync(this List<IFormFile> file, string WebRootPath)
        {
            List<string> pictures = new();
            for (int i = 0; i < file.Count; i++)
            {
                pictures.Add(await file[i].SaveFileAsync(WebRootPath));
            }
            return pictures;
        }
    }
}
