import { useState, useEffect } from "react";
import { apiService } from "../../services/apiService";
import { SportsCardResponse } from "../../types/index";
import { LoadingSpinner } from "../../components/LoadingSpinner";
import { ErrorMessage } from "../../components/ErrorMessage";

interface Stats {
  totalCards: number;
  availableCards: number;
  unavailableCards: number;
  missingImages: number;
}

export function AdminDashboardPage() {
  const [stats, setStats] = useState<Stats>({
    totalCards: 0,
    availableCards: 0,
    unavailableCards: 0,
    missingImages: 0,
  });
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        setIsLoading(true);
        setError(null);

        // Fetch all cards with a large page size
        const response = await apiService.getSportsCards({
          pageSize: 1000,
          page: 1,
        });

        const cards = response.items;
        const totalCards = response.totalCount;
        const availableCards = cards.filter(
          (card: SportsCardResponse) => card.isAvailable,
        ).length;
        const unavailableCards = cards.filter(
          (card: SportsCardResponse) => !card.isAvailable,
        ).length;
        const missingImages = cards.filter(
          (card: SportsCardResponse) =>
            !card.imageUrl || card.imageUrl.trim() === "",
        ).length;

        setStats({
          totalCards,
          availableCards,
          unavailableCards,
          missingImages,
        });
      } catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : "Failed to fetch dashboard stats",
        );
      } finally {
        setIsLoading(false);
      }
    };

    fetchStats();
  }, []);

  if (isLoading) {
    return (
      <div className="p-6">
        <LoadingSpinner />
      </div>
    );
  }

  if (error) {
    return (
      <div className="p-6">
        <ErrorMessage message={error} />
      </div>
    );
  }

  return (
    <div className="p-6">
      <div className="max-w-7xl mx-auto">
        <h2 className="text-2xl font-bold text-gray-900 mb-8">Dashboard</h2>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {/* Total Cards */}
          <div className="bg-blue-50 p-6 rounded-lg border border-blue-200">
            <div className="flex items-center">
              <div className="flex-1">
                <p className="text-sm font-medium text-blue-600">Total Cards</p>
                <p className="text-3xl font-bold text-blue-900">
                  {stats.totalCards}
                </p>
              </div>
            </div>
          </div>

          {/* Available Cards */}
          <div className="bg-green-50 p-6 rounded-lg border border-green-200">
            <div className="flex items-center">
              <div className="flex-1">
                <p className="text-sm font-medium text-green-600">Available</p>
                <p className="text-3xl font-bold text-green-900">
                  {stats.availableCards}
                </p>
              </div>
            </div>
          </div>

          {/* Unavailable Cards */}
          <div className="bg-red-50 p-6 rounded-lg border border-red-200">
            <div className="flex items-center">
              <div className="flex-1">
                <p className="text-sm font-medium text-red-600">Unavailable</p>
                <p className="text-3xl font-bold text-red-900">
                  {stats.unavailableCards}
                </p>
              </div>
            </div>
          </div>

          {/* Missing Images */}
          <div className="bg-yellow-50 p-6 rounded-lg border border-yellow-200">
            <div className="flex items-center">
              <div className="flex-1">
                <p className="text-sm font-medium text-yellow-600">
                  Missing Images
                </p>
                <p className="text-3xl font-bold text-yellow-900">
                  {stats.missingImages}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
