import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { apiService } from "../../services/apiService";
import { SportsCardResponse, Category } from "../../types/index";
import { LoadingSpinner } from "../../components/LoadingSpinner";
import { ErrorMessage } from "../../components/ErrorMessage";
import { Pagination } from "../../components/Pagination";
import { ImageUpload } from "../../components/ImageUpload";

export function ManageCardsPage() {
  const [cards, setCards] = useState<SportsCardResponse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [refreshPage, setRefreshPage] = useState(0);

  const pageSize = 20;

  useEffect(() => {
    const fetchCards = async () => {
      try {
        setIsLoading(true);
        setError(null);

        const response = await apiService.getSportsCards({
          page: currentPage,
          pageSize: pageSize,
        });

        setCards(response.items);
        setTotalPages(response.totalPages);
        setTotalCount(response.totalCount);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to fetch cards");
      } finally {
        setIsLoading(false);
      }
    };

    fetchCards();
  }, [currentPage, refreshPage]);

  const handleDelete = async (id: number, playerName: string) => {
    if (
      !window.confirm(
        `Are you sure you want to delete the card for ${playerName}?`,
      )
    ) {
      return;
    }

    try {
      await apiService.deleteSportsCard(id);
      // Refresh the page to show updated data
      setRefreshPage((prev) => prev + 1);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete card");
    }
  };

  const handleImageUploadSuccess = () => {
    // Refresh the page to show updated images
    setRefreshPage((prev) => prev + 1);
  };

  const getSportName = (sport: Category): string => {
    switch (sport) {
      case Category.Baseball:
        return "Baseball";
      case Category.Basketball:
        return "Basketball";
      case Category.Football:
        return "Football";
      case Category.Hockey:
        return "Hockey";
      default:
        return "Other";
    }
  };

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
        <div className="flex justify-between items-center mb-8">
          <h2 className="text-2xl font-bold text-gray-900">Manage Cards</h2>
          <Link
            to="/admin/cards/new"
            className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700"
          >
            Add New Card
          </Link>
        </div>

        <div className="bg-white shadow overflow-hidden sm:rounded-md">
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Player
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Year
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Brand
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Set
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Condition
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Price
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Available
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Image
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {cards.map((card) => (
                  <tr key={card.id}>
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                      {card.playerName}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {card.year}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {card.brand}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {card.setName}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {card.condition || "N/A"}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      ${card.price.toFixed(2)}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      {card.isAvailable ? (
                        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                          Yes
                        </span>
                      ) : (
                        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-red-100 text-red-800">
                          No
                        </span>
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      {card.imageUrl ? (
                        <img
                          src={card.imageUrl}
                          alt={card.playerName}
                          className="h-12 w-8 object-cover rounded"
                        />
                      ) : (
                        <div className="h-12 w-8 bg-gray-200 rounded flex items-center justify-center">
                          <span className="text-gray-400 text-xs">
                            No Image
                          </span>
                        </div>
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      <div className="flex flex-col space-y-2">
                        <Link
                          to={`/admin/cards/${card.id}/edit`}
                          className="text-blue-600 hover:text-blue-900"
                        >
                          Edit
                        </Link>
                        <button
                          onClick={() => handleDelete(card.id, card.playerName)}
                          className="text-red-600 hover:text-red-900 text-left"
                        >
                          Delete
                        </button>
                        <ImageUpload
                          cardId={card.id}
                          onUploadSuccess={handleImageUploadSuccess}
                          className="text-xs"
                        />
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>

        {totalPages > 1 && (
          <div className="mt-6 flex justify-center">
            <Pagination
              currentPage={currentPage}
              totalPages={totalPages}
              onPageChange={setCurrentPage}
            />
          </div>
        )}

        <div className="mt-4 text-sm text-gray-500">
          Showing {cards.length} of {totalCount} cards
        </div>
      </div>
    </div>
  );
}
