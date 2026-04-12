import { useState, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { PagedSportsCardResponse, CardFilters } from "../types";
import { apiService } from "../services/apiService";
import { LoadingSpinner } from "../components/LoadingSpinner";
import { ErrorMessage } from "../components/ErrorMessage";
import { CardFilters as CardFiltersComponent } from "../components/CardFilters";
import { CardItem } from "../components/CardItem";
import { Pagination } from "../components/Pagination";

export const CardListPage = () => {
  const [cards, setCards] = useState<PagedSportsCardResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [filters, setFilters] = useState<CardFilters>({
    page: 1,
    pageSize: 12,
  });

  const fetchCards = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const result = await apiService.getSportsCards(filters);
      setCards(result);
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Failed to load sports cards",
      );
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchCards();
  }, [fetchCards]);

  const handleFiltersChange = (newFilters: CardFilters) => {
    setFilters({ ...newFilters, page: 1, pageSize: 12 });
  };

  const handleClearFilters = () => {
    setFilters({ page: 1, pageSize: 12 });
  };

  const handlePageChange = (page: number) => {
    setFilters((prev) => ({ ...prev, page }));
    // Scroll to top when page changes
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <LoadingSpinner message="Loading sports cards..." />
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <ErrorMessage message={error} onRetry={fetchCards} />
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header */}
        <div className="mb-6">
          <div className="flex justify-between items-center">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">
                Sports Card Store
              </h1>
              <p className="text-gray-600 mt-2">
                Discover and collect your favorite sports cards
              </p>
            </div>
            {localStorage.getItem("isAdmin") === "true" && (
              <Link
                to="/admin"
                className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-blue-600 bg-blue-100 hover:bg-blue-200"
              >
                Admin
              </Link>
            )}
          </div>
        </div>

        {/* Filters */}
        <div className="mb-6">
          <CardFiltersComponent
            filters={filters}
            onFiltersChange={handleFiltersChange}
            onClearFilters={handleClearFilters}
          />
        </div>

        {/* Results Summary */}
        {cards && (
          <div className="mb-4">
            <p className="text-sm text-gray-600">
              Showing {cards.items.length} of {cards.totalCount} cards
              {cards.totalPages > 1 && (
                <span>
                  {" "}
                  (Page {cards.page} of {cards.totalPages})
                </span>
              )}
            </p>
          </div>
        )}

        {/* Cards Grid */}
        {cards && cards.items.length > 0 ? (
          <>
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6 mb-8">
              {cards.items.map((card) => (
                <CardItem key={card.id} card={card} />
              ))}
            </div>

            {/* Pagination */}
            {cards.totalPages > 1 && (
              <Pagination
                currentPage={cards.page}
                totalPages={cards.totalPages}
                hasNextPage={cards.hasNextPage}
                hasPreviousPage={cards.hasPreviousPage}
                onPageChange={handlePageChange}
              />
            )}
          </>
        ) : (
          <div className="text-center py-12">
            <svg
              className="mx-auto h-12 w-12 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 48 48"
              aria-hidden="true"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M34 40h10v-4a6 6 0 00-10.712-3.714M34 40H14m20 0v-4a9.971 9.971 0 00-.712-3.714M14 40H4v-4a6 6 0 0110.713-3.714M14 40v-4c0-1.313.253-2.566.713-3.714m0 0A10.003 10.003 0 0124 26c4.21 0 7.813 2.602 9.288 6.286"
              />
            </svg>
            <h3 className="mt-2 text-sm font-medium text-gray-900">
              No cards found
            </h3>
            <p className="mt-1 text-sm text-gray-500">
              Try adjusting your filters to find what you're looking for.
            </p>
          </div>
        )}
      </div>
    </div>
  );
};
