import { Link } from "react-router-dom";
import { SportsCardResponse } from "../types";
import {
  formatCategory,
  formatGradingCompany,
  formatPrice,
  formatGrade,
} from "../utils/formatters";

interface CardItemProps {
  card: SportsCardResponse;
}

export const CardItem = ({ card }: CardItemProps) => {
  return (
    <Link to={`/cards/${card.id}`} className="block">
      <div className="bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow duration-200 overflow-hidden">
        {/* Card Image */}
        <div className="aspect-w-3 aspect-h-4 bg-gray-100">
          {card.imageUrl ? (
            <img
              src={card.imageUrl}
              alt={`${card.playerName} ${card.year} ${card.brand}`}
              className="w-full h-48 object-cover"
              loading="lazy"
            />
          ) : (
            <div className="w-full h-48 flex items-center justify-center bg-gray-200">
              <svg
                className="w-12 h-12 text-gray-400"
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
        </div>

        {/* Card Content */}
        <div className="p-4">
          {/* Player Name and Year */}
          <h3 className="text-lg font-semibold text-gray-900 mb-1">
            {card.playerName}
          </h3>

          {/* Brand, Set, Year */}
          <p className="text-sm text-gray-600 mb-2">
            {card.year} {card.brand} {card.setName}
          </p>

          {/* Card Number and Sport */}
          <div className="flex justify-between items-center mb-2">
            <span className="text-sm text-gray-500">#{card.cardNumber}</span>
            <span className="text-xs bg-primary-100 text-primary-800 px-2 py-1 rounded-full">
              {formatCategory(card.sport)}
            </span>
          </div>

          {/* Parallel and Print Run */}
          {(card.parallelName || card.printRun) && (
            <div className="mb-2">
              {card.parallelName && (
                <span className="inline-block text-xs bg-yellow-100 text-yellow-800 px-2 py-1 rounded-full mr-2">
                  {card.parallelName}
                </span>
              )}
              {card.printRun && (
                <span className="inline-block text-xs bg-gray-100 text-gray-800 px-2 py-1 rounded-full">
                  /{card.printRun}
                </span>
              )}
            </div>
          )}

          {/* Grading Information */}
          <div className="flex justify-between items-center mb-3">
            <span className="text-sm text-gray-600">
              {formatGradingCompany(card.gradingCompany)}{" "}
              {formatGrade(card.grade)}
            </span>
            {card.condition && (
              <span className="text-xs text-gray-500">{card.condition}</span>
            )}
          </div>

          {/* Price */}
          <div className="flex justify-between items-center">
            <span className="text-lg font-bold text-green-600">
              {formatPrice(card.price)}
            </span>

            {/* Special badges */}
            <div className="flex space-x-1">
              {card.isRookie && (
                <span className="text-xs bg-blue-100 text-blue-800 px-1 py-0.5 rounded">
                  RC
                </span>
              )}
              {card.isAutograph && (
                <span className="text-xs bg-purple-100 text-purple-800 px-1 py-0.5 rounded">
                  AUTO
                </span>
              )}
              {card.isRelic && (
                <span className="text-xs bg-orange-100 text-orange-800 px-1 py-0.5 rounded">
                  RELIC
                </span>
              )}
              {card.isBowmanFirst && (
                <span className="text-xs bg-red-100 text-red-800 px-1 py-0.5 rounded">
                  1ST
                </span>
              )}
            </div>
          </div>

          {/* Availability */}
          {!card.isAvailable && (
            <div className="mt-2">
              <span className="text-xs bg-red-100 text-red-800 px-2 py-1 rounded-full">
                Sold Out
              </span>
            </div>
          )}
        </div>
      </div>
    </Link>
  );
};
