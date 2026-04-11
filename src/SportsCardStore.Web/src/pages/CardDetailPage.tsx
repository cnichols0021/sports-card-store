import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { SportsCardResponse } from "../types";
import { apiService } from "../services/apiService";
import { LoadingSpinner } from "../components/LoadingSpinner";
import { ErrorMessage } from "../components/ErrorMessage";
import { ImageUpload } from "../components/ImageUpload";
import {
  formatCategory,
  formatGradingCompany,
  formatPrice,
  formatGrade,
} from "../utils/formatters";

export const CardDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [card, setCard] = useState<SportsCardResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [uploadError, setUploadError] = useState<string | null>(null);

  useEffect(() => {
    const fetchCard = async () => {
      if (!id) {
        setError("Card ID not provided");
        setLoading(false);
        return;
      }

      try {
        setLoading(true);
        setError(null);
        const result = await apiService.getSportsCard(Number(id));
        setCard(result);
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "Failed to load card details",
        );
      } finally {
        setLoading(false);
      }
    };

    fetchCard();
  }, [id]);

  const handleBack = () => {
    navigate("/");
  };

  const handleUploadSuccess = (updatedCard: SportsCardResponse) => {
    setCard(updatedCard);
    setUploadError(null);
  };

  const handleUploadError = (message: string) => {
    setUploadError(message);
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <LoadingSpinner message="Loading card details..." />
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <button
            onClick={handleBack}
            className="mb-4 inline-flex items-center text-primary-600 hover:text-primary-500"
          >
            <svg
              className="w-5 h-5 mr-2"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M10 19l-7-7m0 0l7-7m-7 7h18"
              />
            </svg>
            Back to Cards
          </button>
          <ErrorMessage message={error} />
        </div>
      </div>
    );
  }

  if (!card) {
    return null;
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Back Button */}
        <button
          onClick={handleBack}
          className="mb-6 inline-flex items-center text-primary-600 hover:text-primary-500 transition-colors duration-200"
        >
          <svg
            className="w-5 h-5 mr-2"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M10 19l-7-7m0 0l7-7m-7 7h18"
            />
          </svg>
          Back to Cards
        </button>

        <div className="bg-white rounded-lg shadow-lg overflow-hidden">
          <div className="md:flex">
            {/* Card Image */}
            <div className="md:w-1/2 relative">
              {card.imageUrl ? (
                <img
                  src={card.imageUrl}
                  alt={`${card.playerName} ${card.year} ${card.brand}`}
                  className="w-full h-96 md:h-full object-cover"
                />
              ) : (
                <div className="w-full h-96 md:h-full flex items-center justify-center bg-gray-200">
                  <svg
                    className="w-24 h-24 text-gray-400"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
                    />
                  </svg>
                </div>
              )}
              {/* Upload button overlaid on image area */}
              <div className="absolute bottom-3 right-3">
                <ImageUpload
                  cardId={card.id}
                  hasImage={!!card.imageUrl}
                  onUploadSuccess={handleUploadSuccess}
                  onError={handleUploadError}
                />
              </div>
            </div>

            {/* Card Details */}
            <div className="md:w-1/2 p-8">
              {/* Upload error */}
              {uploadError && (
                <div className="mb-4">
                  <ErrorMessage message={uploadError} />
                </div>
              )}

              {/* Header */}
              <div className="mb-6">
                <h1 className="text-3xl font-bold text-gray-900 mb-2">
                  {card.playerName}
                </h1>
                <p className="text-xl text-gray-600">
                  {card.year} {card.brand} {card.setName}
                </p>
                <p className="text-lg text-gray-500 mt-1">
                  Card #{card.cardNumber} • {formatCategory(card.sport)}
                </p>
              </div>

              {/* Special Badges */}
              <div className="flex flex-wrap gap-2 mb-6">
                {card.isRookie && (
                  <span className="px-3 py-1 bg-blue-100 text-blue-800 text-sm font-semibold rounded-full">
                    Rookie Card
                  </span>
                )}
                {card.isAutograph && (
                  <span className="px-3 py-1 bg-purple-100 text-purple-800 text-sm font-semibold rounded-full">
                    Autograph
                  </span>
                )}
                {card.isRelic && (
                  <span className="px-3 py-1 bg-orange-100 text-orange-800 text-sm font-semibold rounded-full">
                    Relic/Memorabilia
                  </span>
                )}
                {card.isBowmanFirst && (
                  <span className="px-3 py-1 bg-red-100 text-red-800 text-sm font-semibold rounded-full">
                    Bowman 1st
                  </span>
                )}
                {card.parallelName && (
                  <span className="px-3 py-1 bg-yellow-100 text-yellow-800 text-sm font-semibold rounded-full">
                    {card.parallelName}
                  </span>
                )}
              </div>

              {/* Price */}
              <div className="mb-6">
                <div className="text-3xl font-bold text-green-600 mb-2">
                  {formatPrice(card.price)}
                </div>
                <div className="flex items-center gap-4">
                  <span className="text-sm text-gray-600">
                    Quantity: {card.quantity}
                  </span>
                  <span
                    className={`px-2 py-1 rounded-full text-xs font-medium ${
                      card.isAvailable
                        ? "bg-green-100 text-green-800"
                        : "bg-red-100 text-red-800"
                    }`}
                  >
                    {card.isAvailable ? "Available" : "Sold Out"}
                  </span>
                </div>
              </div>

              {/* Card Details Grid */}
              <div className="grid grid-cols-2 gap-4 mb-6">
                <div>
                  <h3 className="text-sm font-medium text-gray-500 mb-1">
                    Team
                  </h3>
                  <p className="text-base text-gray-900">{card.team}</p>
                </div>

                <div>
                  <h3 className="text-sm font-medium text-gray-500 mb-1">
                    Grading
                  </h3>
                  <p className="text-base text-gray-900">
                    {formatGradingCompany(card.gradingCompany)}{" "}
                    {formatGrade(card.grade)}
                  </p>
                </div>

                {card.condition && (
                  <div>
                    <h3 className="text-sm font-medium text-gray-500 mb-1">
                      Condition
                    </h3>
                    <p className="text-base text-gray-900">{card.condition}</p>
                  </div>
                )}

                {card.printRun && (
                  <div>
                    <h3 className="text-sm font-medium text-gray-500 mb-1">
                      Print Run
                    </h3>
                    <p className="text-base text-gray-900">/{card.printRun}</p>
                  </div>
                )}
              </div>

              {/* Description */}
              {card.description && (
                <div className="mb-6">
                  <h3 className="text-sm font-medium text-gray-500 mb-2">
                    Description
                  </h3>
                  <p className="text-gray-900 leading-relaxed">
                    {card.description}
                  </p>
                </div>
              )}

              {/* Card Information */}
              <div className="border-t pt-6">
                <h3 className="text-lg font-semibold text-gray-900 mb-4">
                  Card Information
                </h3>
                <div className="space-y-3">
                  <div className="flex justify-between">
                    <span className="text-gray-600">Card ID:</span>
                    <span className="font-medium">#{card.id}</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-600">Listed:</span>
                    <span className="font-medium">
                      {new Date(card.createdDate).toLocaleDateString()}
                    </span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-600">Last Updated:</span>
                    <span className="font-medium">
                      {new Date(card.updatedDate).toLocaleDateString()}
                    </span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
