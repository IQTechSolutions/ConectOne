using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FilingModule.Infrastucture.Implementation
{
    /// <summary>
    /// Provides functionality for processing and saving images from base64-encoded strings.
    /// </summary>
    /// <remarks>This service is designed to handle image processing tasks, such as resizing and saving images
    /// to a specified directory. It supports images encoded as base64 strings and ensures that the resulting image is
    /// saved in a valid format.</remarks>
    public class ImageProcessingService(IRepository<Domain.Entities.Image, string> imageRepository): IImageProcessingService
    {
        /// <summary>
        /// Retrieves all images from the repository.
        /// </summary>
        /// <remarks>The method retrieves all images from the repository and converts them to <see
        /// cref="ImageDto"/> objects. If the operation is unsuccessful, the result will include the failure
        /// messages.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// with a collection of <see cref="ImageDto"/> objects representing the images. If the operation fails, the
        /// result contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<ImageDto>>> AllImagesAsync(CancellationToken cancellationToken = default)
        {
            var result = await imageRepository.ListAsync(false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<ImageDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<ImageDto>>.SuccessAsync(result.Data.Select(c => ImageDto.ToDto(c)));
        }

        /// <summary>
        /// Uploads an image from a Base64-encoded string, processes it, and saves it to the server.
        /// </summary>
        /// <remarks>The method validates the Base64 string, decodes it into an image, and resizes the
        /// image if its width exceeds 1024 pixels. The processed image is saved to a predefined directory on the
        /// server. Metadata about the image, such as its name, format, and size, is stored in the database. If the
        /// operation fails at any step, an appropriate error result is returned.</remarks>
        /// <param name="model">The request model containing the Base64-encoded image string, image metadata, and additional upload details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="ImageDto"/>. The result includes the details of the uploaded image if the
        /// operation succeeds, or error messages if it fails.</returns>
        public async Task<IBaseResult<ImageDto>> UploadImage(Base64ImageUploadRequest model, CancellationToken cancellationToken = default)
        {
            var path = Path.Combine("StaticFiles", "ImageUploads");
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path)))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path));

            if (!model.Base64String.IsBase64String()) return null;

            var data = model.Base64String.GetBase64String();

            using Image image = Image.Load(data);

            string newName = Path.ChangeExtension(Path.GetRandomFileName(), image.Metadata.DecodedImageFormat.Name.ToLower());
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path, newName);

            if (image.Width > 1024)
            {
                image.Mutate(s => s.Resize(1024, 1024 * image.Height / image.Width));
                image.Save(uploadPath);
            }
            else
            {
                image.Save(uploadPath);
            }

            var file = new Domain.Entities.Image
            {
                DisplayName = model.Name,
                FileName = newName,
                ContentType = image.Metadata.DecodedImageFormat.Name.ToLower(),
                Size = image.Height * image.Width,
                RelativePath = Path.Combine(path, newName),
                ImageType = model.ImageType
            };

            var imageResult = await imageRepository.CreateAsync(file, cancellationToken);
            if (!imageResult.Succeeded)
                return await Result<ImageDto>.FailAsync(imageResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<ImageDto>.FailAsync(saveResult.Messages);

            return await Result<ImageDto>.SuccessAsync(ImageDto.ToDto(file));
        }

        /// <summary>
        /// Uploads an image to the repository and persists it.
        /// </summary>
        /// <remarks>This method validates and saves the provided image data to the repository. If the
        /// operation fails at any stage, the result will contain error messages describing the failure.</remarks>
        /// <param name="model">The image data to upload, including metadata such as name, file name, content type, size, and relative path.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="ImageDto"/>. The result indicates whether the operation succeeded or failed,
        /// and includes the uploaded image data if successful.</returns>
        public async Task<IBaseResult<ImageDto>> UploadImage(ImageDto model, CancellationToken cancellationToken = default)
        {
            var file = new Domain.Entities.Image
            {
                Id = model.Id,
                DisplayName = model.Name,
                FileName = model.FileName,
                ContentType = model.ContentType,
                Size = model.Size,
                RelativePath = model.RelativePath,
                ImageType = model.ImageType.Value
            };

            var imageResult = await imageRepository.CreateAsync(file, cancellationToken);
            if (!imageResult.Succeeded)
                return await Result<ImageDto>.FailAsync(imageResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<ImageDto>.FailAsync(saveResult.Messages);

            return await Result<ImageDto>.SuccessAsync(ImageDto.ToDto(file));
        }

        /// <summary>
        /// Uploads an image file to the server and saves its metadata to the database.
        /// </summary>
        /// <remarks>The uploaded file is saved to the server's file system under a unique name to avoid
        /// collisions. The maximum allowed file size is 1 GB. If the file exceeds this limit, the operation
        /// fails.</remarks>
        /// <param name="model">The request model containing the image file, its metadata, and upload details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with an <see cref="ImageDto"/> representing the uploaded image if the operation succeeds, or error messages
        /// if it fails.</returns>
        public async Task<IBaseResult<ImageDto>> UploadImage(ImageUploadRequest model, CancellationToken cancellationToken = default)
        {
            // OPTIONAL: server-side length check
            const long maxBytes = 1_000_000_000; // 1 GB
            if (model.File.Length > maxBytes)
                return await Result<ImageDto>.FailAsync("File too large (max 1 GB)");

            var path = Path.Combine("StaticFiles", "ImageUploads");
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
            Directory.CreateDirectory(rootPath);

            // Use a GUID to avoid name collisions
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";
            var filePath = Path.Combine(rootPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.File.CopyToAsync(stream);

            var file = new Domain.Entities.Image
            {
                DisplayName = model.Name,
                FileName = fileName,
                ContentType = model.File.ContentType,
                RelativePath = Path.Combine(path, fileName),
                ImageType = model.ImageType
            };

            var imageResult = await imageRepository.CreateAsync(file, cancellationToken);
            if (!imageResult.Succeeded)
                return await Result<ImageDto>.FailAsync(imageResult.Messages);

            var saveResult = await imageRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<ImageDto>.FailAsync(saveResult.Messages);

            return await Result<ImageDto>.SuccessAsync(ImageDto.ToDto(file));
        }

        /// <summary>
        /// Retrieves information about a specified file.
        /// </summary>
        /// <remarks>The method checks for the existence of the file in the
        /// "wwwroot/StaticFiles/ImageUploads" directory. If the file exists, its metadata is returned; otherwise, a
        /// failure result is returned.</remarks>
        /// <param name="fileName">The name of the file to retrieve information for. This cannot be null or empty.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// where <c>T</c> is <see cref="FileInfoResponse"/>. If the file is found, the result contains the file's name,
        /// size, and creation time. If the file is not found, the result indicates failure with an appropriate message.</returns>
        public async Task<IBaseResult<FileInfoResponse>> GetInfoAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var path = Path.Combine("wwwroot", "StaticFiles", "ImageUploads", fileName);
            if (File.Exists(path))
                return await Result<FileInfoResponse>.FailAsync("File not found");

            var info = new FileInfo(path);
            var dto = new FileInfoResponse(info.Name, info.Length, info.CreationTimeUtc);

            return await Result<FileInfoResponse>.SuccessAsync(dto);
        }

        /// <summary>
        /// Deletes an image with the specified identifier from the repository and the file system.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="number"> <item>Retrieves the
        /// image from the repository using the specified <paramref name="imageId"/>.</item> <item>Deletes the image
        /// record from the repository if it exists.</item> <item>Saves the changes to the repository.</item>
        /// <item>Deletes the associated image file from the file system, if it exists.</item> </list> If any step
        /// fails, the operation is aborted, and the result contains the corresponding error messages.</remarks>
        /// <param name="imageId">The unique identifier of the image to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. If the operation fails, the result includes error
        /// messages.</returns>
        public async Task<IBaseResult> DeleteImageAsync(string imageId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Domain.Entities.Image>(c => c.Id == imageId);
            var imageResult = await imageRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!imageResult.Succeeded)
                return await Result.FailAsync(imageResult.Messages);

            var deleteResult = await imageRepository.DeleteAsync(imageId, cancellationToken);
            if (!deleteResult.Succeeded)
                return await Result.FailAsync(deleteResult.Messages);

            var saveResult = await imageRepository.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            try
            {
                if (File.Exists(Path.Combine("wwwroot", imageResult.Data.RelativePath)))
                    File.Delete(Path.Combine("wwwroot", imageResult.Data.RelativePath));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return await Result.SuccessAsync();
        }
    }
}
