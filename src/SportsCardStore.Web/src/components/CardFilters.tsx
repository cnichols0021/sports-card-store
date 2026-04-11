import { useState } from "react";
import { Category, CardFilters as CardFiltersType } from "../types";
import { formatCategory } from "../utils/formatters";

interface CardFiltersProps {
  filters: CardFiltersType;
  onFiltersChange: (filters: CardFiltersType) => void;
  onClearFilters: () => void;
}

export const CardFilters = ({
  filters,
  onFiltersChange,
  onClearFilters,
}: CardFiltersProps) => {
  const [localFilters, setLocalFilters] = useState<CardFiltersType>(filters);

  const handleInputChange = (
    field: keyof CardFiltersType,
    value: string | number | boolean | undefined,
  ) => {
    const newFilters = { ...localFilters, [field]: value };
    setLocalFilters(newFilters);
    onFiltersChange(newFilters);
  };

  const handleClearFilters = () => {
    const clearedFilters: CardFiltersType = { page: 1, pageSize: 12 };
    setLocalFilters(clearedFilters);
    onClearFilters();
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-md">
      <h2 className="text-lg font-semibold text-gray-900 mb-4">Filter Cards</h2>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {/* Sport Filter */}
        <div>
          <label
            htmlFor="sport"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Sport
          </label>
          <select
            id="sport"
            value={localFilters.sport || ""}
            onChange={(e) =>
              handleInputChange(
                "sport",
                e.target.value
                  ? (Number(e.target.value) as Category)
                  : undefined,
              )
            }
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          >
            <option value="">All Sports</option>
            {Object.values(Category)
              .filter((value) => typeof value === "number")
              .map((category) => (
                <option key={category} value={category}>
                  {formatCategory(category as Category)}
                </option>
              ))}
          </select>
        </div>

        {/* Brand Filter */}
        <div>
          <label
            htmlFor="brand"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Brand
          </label>
          <input
            id="brand"
            type="text"
            value={localFilters.brand || ""}
            onChange={(e) =>
              handleInputChange("brand", e.target.value || undefined)
            }
            placeholder="e.g., Topps, Panini"
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          />
        </div>

        {/* Player Name Filter */}
        <div>
          <label
            htmlFor="playerName"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Player Name
          </label>
          <input
            id="playerName"
            type="text"
            value={localFilters.playerName || ""}
            onChange={(e) =>
              handleInputChange("playerName", e.target.value || undefined)
            }
            placeholder="e.g., Mike Trout"
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          />
        </div>

        {/* Bowman First Filter */}
        <div className="flex items-center">
          <input
            id="bowmanFirst"
            type="checkbox"
            checked={localFilters.isBowmanFirst || false}
            onChange={(e) =>
              handleInputChange("isBowmanFirst", e.target.checked || undefined)
            }
            className="h-4 w-4 text-primary-600 focus:ring-primary-500 border-gray-300 rounded"
          />
          <label
            htmlFor="bowmanFirst"
            className="ml-2 block text-sm font-medium text-gray-700"
          >
            Bowman First
          </label>
        </div>

        {/* Is Autograph Filter */}
        <div className="flex items-center">
          <input
            id="isAutograph"
            type="checkbox"
            checked={localFilters.isAutograph || false}
            onChange={(e) =>
              handleInputChange("isAutograph", e.target.checked || undefined)
            }
            className="h-4 w-4 text-primary-600 focus:ring-primary-500 border-gray-300 rounded"
          />
          <label
            htmlFor="isAutograph"
            className="ml-2 block text-sm font-medium text-gray-700"
          >
            Autograph
          </label>
        </div>

        {/* Min Price Filter */}
        <div>
          <label
            htmlFor="minPrice"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Min Price ($)
          </label>
          <input
            id="minPrice"
            type="number"
            min="0"
            step="0.01"
            value={localFilters.minPrice || ""}
            onChange={(e) =>
              handleInputChange(
                "minPrice",
                e.target.value ? Number(e.target.value) : undefined,
              )
            }
            placeholder="0.00"
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          />
        </div>

        {/* Max Price Filter */}
        <div>
          <label
            htmlFor="maxPrice"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Max Price ($)
          </label>
          <input
            id="maxPrice"
            type="number"
            min="0"
            step="0.01"
            value={localFilters.maxPrice || ""}
            onChange={(e) =>
              handleInputChange(
                "maxPrice",
                e.target.value ? Number(e.target.value) : undefined,
              )
            }
            placeholder="100000.00"
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          />
        </div>

        {/* Parallel Name Filter */}
        <div>
          <label
            htmlFor="parallelName"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Parallel Name
          </label>
          <input
            id="parallelName"
            type="text"
            value={localFilters.parallelName || ""}
            onChange={(e) =>
              handleInputChange("parallelName", e.target.value || undefined)
            }
            placeholder="e.g., Gold, Refractor"
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          />
        </div>

        {/* Max Print Run Filter */}
        <div>
          <label
            htmlFor="maxPrintRun"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Max Print Run
          </label>
          <input
            id="maxPrintRun"
            type="number"
            min="1"
            value={localFilters.maxPrintRun || ""}
            onChange={(e) =>
              handleInputChange(
                "maxPrintRun",
                e.target.value ? Number(e.target.value) : undefined,
              )
            }
            placeholder="e.g., 99, 500"
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
          />
        </div>
      </div>

      {/* Clear Filters Button */}
      <div className="mt-6">
        <button
          onClick={handleClearFilters}
          className="px-4 py-2 bg-gray-200 hover:bg-gray-300 text-gray-800 text-sm font-medium rounded-md transition-colors duration-200"
        >
          Clear All Filters
        </button>
      </div>
    </div>
  );
};
