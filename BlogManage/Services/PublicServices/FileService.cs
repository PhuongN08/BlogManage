namespace BlogManage.Services.PublicServices
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            // Kiểm tra định dạng file
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Chỉ cho phép các định dạng ảnh: JPG, JPEG, PNG, GIF.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Giữ tên gốc của ảnh
            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var safeFileName = string.Join("_", originalFileName.Split(Path.GetInvalidFileNameChars()))
                                    .Replace(" ", "_");

            // Tránh trùng tên bằng cách thêm thời gian (hoặc bỏ nếu bạn cho phép trùng)
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{safeFileName}_{timestamp}{fileExtension}";

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn tương đối để lưu vào DB
            return $"/{folderName}/{fileName}";
        }


        public bool DeleteImage(string relativePath)
        {
            var filePath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
    }
}
