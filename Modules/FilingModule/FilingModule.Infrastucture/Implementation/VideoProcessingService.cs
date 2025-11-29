using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace FilingModule.Infrastucture.Implementation
{
    /// <summary>
    /// Provides services for processing video files, including uploading, retrieving metadata, and deleting videos.
    /// </summary>
    /// <remarks>This service handles video file operations such as uploading files to the server, saving
    /// metadata to a repository, retrieving file information, and deleting videos. Uploaded files are stored in the
    /// "wwwroot/StaticFiles/UploadedVideos" directory, and unique file names are generated to prevent name collisions.
    /// The service enforces a maximum file size of 1 GB for uploads and interacts with a repository to persist video
    /// metadata.</remarks>
    /// <param name="videoRepository"></param>
    public class VideoProcessingService(IRepository<Video, string> videoRepository) : IVideoProcessingService
    {
        /// <summary>
        /// Retrieves all videos from the repository.
        /// </summary>
        /// <remarks>This method retrieves all videos from the repository and converts them into <see
        /// cref="VideoDto"/> objects. If the operation is unsuccessful, the result will indicate failure along with the
        /// associated error messages.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing a collection of <see cref="VideoDto"/> objects representing the videos. If the operation fails,
        /// the result will include error messages.</returns>
        public async Task<IBaseResult<IEnumerable<VideoDto>>> AllVideosAsync(CancellationToken cancellationToken = default)
        {
            var result = await videoRepository.ListAsync(false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<VideoDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<VideoDto>>.SuccessAsync(result.Data.Select(c => VideoDto.ToDto(c)));
        }

        /// <summary>
        /// Uploads a video file to the server and saves its metadata to the repository.
        /// </summary>
        /// <remarks>The uploaded file is saved to the "wwwroot/StaticFiles/UploadedVideos" directory on
        /// the server. The file name is automatically generated using a GUID to avoid name collisions. The maximum
        /// allowed file size is 1 GB.</remarks>
        /// <param name="video">The video upload request containing the file and associated metadata. The file must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a <see cref="VideoUploadResponse"/> object if the upload is successful, or error messages if the
        /// operation fails.</returns>
        public async Task<IBaseResult<VideoUploadResponse>> UploadVideoAsync(VideoUploadRequest video, CancellationToken cancellationToken = default)
        {
            if (video is null || video.File.Length == 0)
                return await Result<VideoUploadResponse>.FailAsync("No video found");

            // OPTIONAL: server-side length check
            const long maxBytes = 1_000_000_000; // 1 GB
            if (video.File.Length > maxBytes)
                return await Result<VideoUploadResponse>.FailAsync("File too large (max 1 GB)");

            var path = Path.Combine("StaticFiles", "UploadedVideos");
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
            Directory.CreateDirectory(rootPath);

            // Use a GUID to avoid name collisions
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(video.File.FileName)}";
            var filePath = Path.Combine(rootPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await video.File.CopyToAsync(stream, cancellationToken);

            var file = new Video()
            {
                Id = Guid.NewGuid().ToString(),
                DisplayName = video.Name,
                FileName = fileName,
                ContentType = video.File.ContentType,
                RelativePath = Path.Combine(path, fileName)
            };

            var videoResult = await videoRepository.CreateAsync(file, cancellationToken);
            if (!videoResult.Succeeded)
                return await Result<VideoUploadResponse>.FailAsync(videoResult.Messages);

            var saveResult = await videoRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<VideoUploadResponse>.FailAsync(saveResult.Messages);

            return await Result<VideoUploadResponse>.SuccessAsync(new VideoUploadResponse(file.Id, fileName, video.File.Length, filePath));
        }

        /// <summary>
        /// Asynchronously uploads a video file to the server and stores it in the designated directory.
        /// </summary>
        /// <remarks>The method performs the following validations: <list type="bullet">
        /// <item><description>The <paramref name="video"/> parameter must not be null or empty.</description></item>
        /// <item><description>The video file size must not exceed 1 GB.</description></item> </list> The uploaded video
        /// is saved to the "wwwroot/StaticFiles/UploadedVideos" directory, and a unique file name is generated to avoid
        /// name collisions. The method interacts with a repository to persist video metadata.</remarks>
        /// <param name="video">The video file to upload. The file must not be null and must have a non-zero length.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that indicates the success or failure of the operation. On success, the result includes details about
        /// the uploaded video, such as its ID, file name, size, and file path.</returns>
        public async Task<IBaseResult<VideoUploadResponse>> UploadVideoAsync(HttpContent video, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            throw new NotImplementedException("Call is reserved for Web Assembly");
        }

        /// <summary>
        /// Uploads a video file to the server and saves its metadata to the repository.
        /// </summary>
        /// <remarks>The uploaded file is saved to the "wwwroot/StaticFiles/UploadedVideos" directory on
        /// the server. The file name is automatically generated using a GUID to avoid name collisions. The maximum
        /// allowed file size is 1 GB.</remarks>
        /// <param name="video">The video upload request containing the file and associated metadata. The file must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a <see cref="VideoUploadResponse"/> object if the upload is successful, or error messages if the
        /// operation fails.</returns>
        public async Task<IBaseResult<VideoUploadResponse>> UploadVideoAsync(IFormFile video, CancellationToken cancellationToken = default)
        {
            if (video is null || video.Length == 0)
                return await Result<VideoUploadResponse>.FailAsync("No video found");

            // OPTIONAL: server-side length check
            const long maxBytes = 1_000_000_000; // 1 GB
            if (video.Length > maxBytes)
                return await Result<VideoUploadResponse>.FailAsync("File too large (max 1 GB)");

            var path = Path.Combine("StaticFiles", "UploadedVideos");
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
            Directory.CreateDirectory(rootPath);

            // Use a GUID to avoid name collisions
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(video.FileName)}";
            var filePath = Path.Combine(rootPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await video.CopyToAsync(stream, cancellationToken);

            var file = new Video()
            {
                Id = Guid.NewGuid().ToString(),
                DisplayName = video.Name,
                FileName = fileName,
                ContentType = video.ContentType,
                RelativePath = Path.Combine(path, fileName)
            };

            var videoResult = await videoRepository.CreateAsync(file, cancellationToken);
            if (!videoResult.Succeeded)
                return await Result<VideoUploadResponse>.FailAsync(videoResult.Messages);

            var saveResult = await videoRepository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded)
                return await Result<VideoUploadResponse>.FailAsync(saveResult.Messages);

            return await Result<VideoUploadResponse>.SuccessAsync(new VideoUploadResponse(file.Id, fileName, video.Length, filePath));
        }

        /// <summary>
        /// Retrieves information about a specified file.
        /// </summary>
        /// <remarks>The method checks for the existence of the file in the predefined directory
        /// structure: "wwwroot/StaticFiles/UploadedVideos". If the file is not found, the operation fails with a "File
        /// not Found" message.</remarks>
        /// <param name="filename">The name of the file to retrieve information for. This must include the file name and extension.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="FileInfoResponse"/>. If the file exists, the result contains the file's name,
        /// size, and creation time in UTC. If the file does not exist, the result indicates failure with an appropriate
        /// error message.</returns>
        public async Task<IBaseResult<FileInfoResponse>> GetInfoAsync(string filename, CancellationToken cancellationToken = default)
        {
            var path = Path.Combine("wwwroot", "StaticFiles", "UploadedVideos", filename);
            if (!File.Exists(path)) return await Result<FileInfoResponse>.FailAsync("File not Found");
            var info = new FileInfo(path);
            return await Result<FileInfoResponse>.SuccessAsync(new FileInfoResponse(info.Name, info.Length, info.CreationTimeUtc));
        }

        /// <summary>
        /// Deletes a video identified by its unique ID and removes its associated file from the file system.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="bullet"> <item>Checks if the
        /// video exists in the repository.</item> <item>Deletes the video record from the repository if it
        /// exists.</item> <item>Saves the changes to the repository.</item> <item>Attempts to delete the associated
        /// video file from the file system.</item> </list> If the video does not exist or any step fails, the method
        /// returns a failure result with appropriate messages.</remarks>
        /// <param name="videoId">The unique identifier of the video to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation, along with any associated messages.</returns>
        public async Task<IBaseResult> DeleteVideoAsync(string videoId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<Domain.Entities.Video>(c => c.Id == videoId);
            var videoResult = await videoRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!videoResult.Succeeded)
                return await Result.FailAsync(videoResult.Messages);

            var deleteResult = await videoRepository.DeleteAsync(videoId, cancellationToken);
            if (!deleteResult.Succeeded)
                return await Result.FailAsync(deleteResult.Messages);

            var saveResult = await videoRepository.SaveAsync();
            if (!saveResult.Succeeded)
                return await Result.FailAsync(saveResult.Messages);

            try
            {
                if (System.IO.File.Exists(Path.Combine("wwwroot", videoResult.Data.RelativePath)))
                    System.IO.File.Delete(Path.Combine("wwwroot", videoResult.Data.RelativePath));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return await Result.SuccessAsync();
        }
    }
}
