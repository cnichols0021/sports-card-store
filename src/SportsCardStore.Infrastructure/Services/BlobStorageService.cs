using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SportsCardStore.Core.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SportsCardStore.Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly ILogger<BlobStorageService> _logger;
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
        {
            _logger = logger;

            try
            {
                // Try nested key first (local dev via user secrets / appsettings)
                // Fall back to flat env var names (Azure Linux App Service portal)
                var connectionString = configuration["AzureBlobStorage:ConnectionString"]
                                    ?? configuration["BLOB_STORAGE_CONNECTION_STRING"];

                _containerName = configuration["AzureBlobStorage:ContainerName"]
                              ?? configuration["BLOB_STORAGE_CONTAINER_NAME"]
                              ?? "card-images";

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(
                        "Blob storage connection string is missing. " +
                        "Set AzureBlobStorage:ConnectionString (local) or " +
                        "BLOB_STORAGE_CONNECTION_STRING (Azure App Service).");
                }

                _blobServiceClient = new BlobServiceClient(connectionString);
                _containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

                _logger.LogInformation("BlobStorageService initialized successfully with container: {ContainerName}", _containerName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize BlobStorageService");
                throw;
            }
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            try
            {
                if (imageStream == null)
                    throw new ArgumentNullException(nameof(imageStream));

                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

                if (imageStream.CanSeek)
                    imageStream.Position = 0;

                var uniqueFileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}_{fileName}";
                var blobClient = _containerClient.GetBlobClient(uniqueFileName);

                var contentType = GetContentType(fileName);
                var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

                await blobClient.UploadAsync(
                    imageStream,
                    new BlobUploadOptions
                    {
                        HttpHeaders = blobHttpHeaders
                    });

                _logger.LogInformation("Successfully uploaded image: {FileName} as {UniqueFileName}", fileName, uniqueFileName);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload image: {FileName}", fileName);
                throw new InvalidOperationException($"Failed to upload image {fileName}", ex);
            }
        }

        public async Task<bool> DeleteImageAsync(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    _logger.LogWarning("Attempted to delete image with null or empty filename");
                    return false;
                }

                var blobClient = _containerClient.GetBlobClient(fileName);
                var deleteResult = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

                if (deleteResult.Value)
                {
                    _logger.LogInformation("Successfully deleted image: {FileName}", fileName);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Image not found for deletion: {FileName}", fileName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete image: {FileName}", fileName);
                return false;
            }
        }

        public async Task<string> GetImageUrlAsync(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

                var blobClient = _containerClient.GetBlobClient(fileName);
                var exists = await blobClient.ExistsAsync();

                if (!exists.Value)
                {
                    _logger.LogWarning("Image not found: {FileName}", fileName);
                    throw new FileNotFoundException($"Image not found: {fileName}");
                }

                _logger.LogDebug("Retrieved URL for image: {FileName}", fileName);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get image URL: {FileName}", fileName);
                throw;
            }
        }

        private static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".tiff" or ".tif" => "image/tiff",
                _ => "application/octet-stream"
            };
        }
    }
}
