using image.Data;
using image.Models;
using Microsoft.EntityFrameworkCore;

namespace image.Services
{
    public class ImageServices
    {
        public static async Task StoreImage(object model, IFormFile picture, IWebHostEnvironment _webHost, DataDbContext _context)
        {
            if (picture != null)
            {
                // Get type name (like "Product")
                var modelName = model.GetType().Name;

                // Get the Id property value
                var idProperty = model.GetType().GetProperty("Id");
                if (idProperty == null)
                {
                    throw new Exception("Model must have an Id property.");
                }
                var idValue = (int)idProperty.GetValue(model);

                // Now continue like before
                var filename = $"{idValue}-{picture.FileName}";
                var directoryPath = Path.Combine(_webHost.WebRootPath, modelName);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var path = Path.Combine(directoryPath, filename);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await picture.CopyToAsync(fileStream);
                }

                var AllMedia = new Media
                {
                    Model = modelName,
                    CollectionName = modelName,
                    Name = filename,
                    IdRelated = idValue,
                    FileSize = picture.Length.ToString(),
                };

                await _context.Medias.AddAsync(AllMedia);
                await _context.SaveChangesAsync();
            }
        }


        public static async Task EditImage(object model, IFormFile picture, IWebHostEnvironment _webHost, DataDbContext _context)
        {
            if (picture != null)
            {
                var modelName = model.GetType().Name;
                var idProperty = model.GetType().GetProperty("Id");
                if (idProperty == null)
                {
                    throw new Exception("Model must have an Id property.");
                }
                var idValue = (int)idProperty.GetValue(model);

                var directoryPath = Path.Combine(_webHost.WebRootPath, modelName);

                // Delete old image if exists
                if (Directory.Exists(directoryPath))
                {
                    var oldFiles = Directory.GetFiles(directoryPath, $"{idValue}-*");
                    foreach (var oldFile in oldFiles)
                    {
                        File.Delete(oldFile);
                    }
                }
                else
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Save new image
                var filename = $"{idValue}-{picture.FileName}";
                var path = Path.Combine(directoryPath, filename);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await picture.CopyToAsync(fileStream);
                }

                // Update Media table: (optional) Delete old media entry
                var oldMedia = await _context.Medias.FirstOrDefaultAsync(x => x.IdRelated == idValue && x.Model == modelName);
                if (oldMedia != null)
                {
                    _context.Medias.Remove(oldMedia);
                    await _context.SaveChangesAsync();
                }

                var newMedia = new Media
                {
                    Model = modelName,
                    CollectionName = modelName,
                    Name = filename,
                    IdRelated = idValue,
                    FileSize = picture.Length.ToString(),
                };

                await _context.Medias.AddAsync(newMedia);
                await _context.SaveChangesAsync();
            }
        }


        public static async Task DeleteImage(object model, IWebHostEnvironment _webHost, DataDbContext _context)
        {
            var modelName = model.GetType().Name;
            var idProperty = model.GetType().GetProperty("Id");
            if (idProperty == null)
            {
                throw new Exception("Model must have an Id property.");
            }
            var idValue = (int)idProperty.GetValue(model);

            var directoryPath = Path.Combine(_webHost.WebRootPath, modelName);

            // Delete physical image files
            if (Directory.Exists(directoryPath))
            {
                var files = Directory.GetFiles(directoryPath, $"{idValue}-*");
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }

            // Delete Media record
            var mediaRecords = _context.Medias.Where(x => x.Model == modelName && x.IdRelated == idValue);
            _context.Medias.RemoveRange(mediaRecords);
            await _context.SaveChangesAsync();
        }


        public static string GetImageUrl(object model, IWebHostEnvironment _webHost)
        {
            var modelName = model.GetType().Name;
            var idProperty = model.GetType().GetProperty("Id");
            if (idProperty == null)
            {
                throw new Exception("Model must have an Id property.");
            }
            var idValue = (int)idProperty.GetValue(model);

            // Find the image file in the folder
            var directoryPath = Path.Combine(_webHost.WebRootPath, modelName);
            if (!Directory.Exists(directoryPath))
            {
                return "/images/default.png"; // fallback image if folder doesn't exist
            }

            var files = Directory.GetFiles(directoryPath, $"{idValue}-*"); // find by id prefix
            if (files.Length == 0)
            {
                return "/images/default.png"; // fallback image if no file
            }

            var fileName = Path.GetFileName(files[0]);
            return $"/{modelName}/{fileName}"; // This creates a relative path like "/Product/1-Capture.PNG"
        }

    }
}
