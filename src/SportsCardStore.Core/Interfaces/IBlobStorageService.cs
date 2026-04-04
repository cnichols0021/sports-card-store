using System.IO;
using System.Threading.Tasks;

namespace SportsCardStore.Core.Interfaces
{
    /// <summary>
    /// Service for managing blob storage operations for sports card images
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Uploads an image to blob storage and returns the public URL
        /// </summary>
        /// <param name="imageStream">The image file stream</param>
        /// <param name="fileName">The name to save the file as</param>
        /// <returns>The public URL of the uploaded image</returns>
        Task<string> UploadImageAsync(Stream imageStream, string fileName);

        /// <summary>
        /// Deletes an image from blob storage
        /// </summary>
        /// <param name="fileName">The name of the file to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteImageAsync(string fileName);

        /// <summary>
        /// Gets the public URL for an image in blob storage
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>The public URL of the image</returns>
        Task<string> GetImageUrlAsync(string fileName);
    }
}