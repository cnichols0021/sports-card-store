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
