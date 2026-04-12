import { useState } from "react";
import { apiService } from "../services/apiService";
import { LoadingSpinner } from "./LoadingSpinner";
import { ErrorMessage } from "./ErrorMessage";

interface ImageUploadProps {
  cardId: number;
  onUploadSuccess?: (imageUrl: string) => void;
  className?: string;
}

export function ImageUpload({
  cardId,
  onUploadSuccess,
  className = "",
}: ImageUploadProps) {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleFileSelect = async (
    event: React.ChangeEvent<HTMLInputElement>,
  ) => {
    const file = event.target.files?.[0];
    if (!file) return;

    // Validate file type
    if (!file.type.startsWith("image/")) {
      setError("Please select an image file");
      return;
    }

    // Validate file size (max 5MB)
    if (file.size > 5 * 1024 * 1024) {
      setError("File size must be less than 5MB");
      return;
    }

    setIsLoading(true);
    setError(null);

    try {
      const result = await apiService.uploadCardImage(cardId, file);
      onUploadSuccess?.(result.imageUrl || "");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to upload image");
    } finally {
      setIsLoading(false);
      // Reset file input
      event.target.value = "";
    }
  };

  return (
    <div className={`relative ${className}`}>
      <input
        type="file"
        accept="image/*"
        onChange={handleFileSelect}
        disabled={isLoading}
        className="hidden"
        id={`file-upload-${cardId}`}
      />
      <label
        htmlFor={`file-upload-${cardId}`}
        className={`inline-block px-3 py-1 text-sm font-medium text-blue-600 bg-blue-100 hover:bg-blue-200 rounded cursor-pointer transition-colors ${
          isLoading ? "opacity-50 cursor-not-allowed" : ""
        }`}
      >
        {isLoading ? (
          <span className="flex items-center">
            <LoadingSpinner size="sm" className="mr-1" />
            Uploading...
          </span>
        ) : (
          "Upload Image"
        )}
      </label>
      {error && <ErrorMessage message={error} className="mt-1" />}
    </div>
  );
}
=======
    // Client-side validation before sending
    const allowedTypes = ["image/jpeg", "image/jpg", "image/png"];
    if (!allowedTypes.includes(file.type)) {
      onError("Only JPG and PNG files are supported.");
      return;
    }

    const maxSize = 10 * 1024 * 1024; // 10MB
    if (file.size > maxSize) {
      onError("File size must be under 10MB.");
      return;
    }

    try {
      setUploading(true);
      const updatedCard = await apiService.uploadCardImage(cardId, file);
      onUploadSuccess(updatedCard);
    } catch (err) {
      onError(
        err instanceof Error ? err.message : "Upload failed. Please try again.",
      );
    } finally {
      setUploading(false);
      // Reset input so the same file can be re-selected if needed
      if (fileInputRef.current) fileInputRef.current.value = "";
    }
  };

  const handleButtonClick = () => {
    fileInputRef.current?.click();
  };

  return (
    <div>
      <input
        ref={fileInputRef}
        type="file"
        accept="image/jpeg,image/jpg,image/png"
        onChange={handleFileChange}
        className="hidden"
        disabled={uploading}
      />
      <button
        onClick={handleButtonClick}
        disabled={uploading}
        className={`inline-flex items-center gap-2 px-4 py-2 rounded-md text-sm font-medium transition-colors duration-200 ${
          uploading
            ? "bg-gray-300 text-gray-500 cursor-not-allowed"
            : hasImage
              ? "bg-white border border-gray-300 text-gray-700 hover:bg-gray-50"
              : "bg-blue-600 text-white hover:bg-blue-700"
        }`}
      >
        {uploading ? (
          <>
            <svg
              className="animate-spin h-4 w-4"
              fill="none"
              viewBox="0 0 24 24"
            >
              <circle
                className="opacity-25"
                cx="12"
                cy="12"
                r="10"
                stroke="currentColor"
                strokeWidth="4"
              />
              <path
                className="opacity-75"
                fill="currentColor"
                d="M4 12a8 8 0 018-8v8H4z"
              />
            </svg>
            Uploading...
          </>
        ) : (
          <>
            <svg
              className="h-4 w-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12"
              />
            </svg>
            {hasImage ? "Replace Image" : "Upload Image"}
          </>
        )}
      </button>
    </div>
  );
};
>>>>>>> 1d06ba01445ca18605e302ae6df45070e501787d
