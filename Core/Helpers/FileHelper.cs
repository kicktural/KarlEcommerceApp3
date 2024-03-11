using Microsoft.AspNetCore.Http;

namespace Core.Helper
{
    public static class FileHelper
    {
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
