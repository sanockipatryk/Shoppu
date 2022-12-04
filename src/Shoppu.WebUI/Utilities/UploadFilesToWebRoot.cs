using System.Text;

namespace Shoppu.WebUI.Utilities
{
    public static class UploadFilesToWebRoot
    {

        public static List<string>? UploadManyFiles(
            IWebHostEnvironment _webHostEnvironment,
            HttpRequest request,
            string folderName,
            params string[] innerFolders
            )
        {
            var files = request.Form.Files;
            if (files.Count > 0)
            {
                int index = 0;
                var imageUrls = new List<string>();

                string webRootPath = _webHostEnvironment.WebRootPath;
                var uploadFolderPath = Path.Combine(webRootPath, folderName);

                var folders = new List<string>();
                folders.Add(uploadFolderPath);
                foreach (var folder in innerFolders)
                    folders.Add(folder);

                var filePath = Path.Combine(folders.ToArray());
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                foreach (var file in files)
                {
                    StringBuilder filePathString = new StringBuilder();
                    var extension = Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(filePath, index + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    filePathString.Append(@$"{folderName}\");
                    foreach (var folder in innerFolders)
                        filePathString.Append(@$"{folder}\");
                    filePathString.Append(index + extension);

                    imageUrls.Add(filePathString.ToString());
                    index++;
                }
                return imageUrls;
            }
            return null;
        }
    }
}
