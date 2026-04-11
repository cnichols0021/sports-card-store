interface PaginationProps {
  currentPage: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  onPageChange: (page: number) => void;
}

export const Pagination = ({
  currentPage,
  totalPages,
  hasNextPage,
  hasPreviousPage,
  onPageChange,
}: PaginationProps) => {
  const handlePrevious = () => {
    if (hasPreviousPage) {
      onPageChange(currentPage - 1);
    }
  };

  const handleNext = () => {
    if (hasNextPage) {
      onPageChange(currentPage + 1);
    }
  };

  return (
    <div className="flex items-center justify-between bg-white px-4 py-3 sm:px-6 rounded-lg shadow-md">
      <div className="flex flex-1 justify-between sm:hidden">
        {/* Mobile pagination */}
        <button
          onClick={handlePrevious}
          disabled={!hasPreviousPage}
          className="relative inline-flex items-center rounded-md bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed border border-gray-300"
        >
          Previous
        </button>
        <button
          onClick={handleNext}
          disabled={!hasNextPage}
          className="relative ml-3 inline-flex items-center rounded-md bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed border border-gray-300"
        >
          Next
        </button>
      </div>

      <div className="hidden sm:flex sm:flex-1 sm:items-center sm:justify-between">
        {/* Desktop pagination info */}
        <div>
          <p className="text-sm text-gray-700">
            Page <span className="font-medium">{currentPage}</span> of{" "}
            <span className="font-medium">{totalPages}</span>
          </p>
        </div>

        {/* Desktop pagination controls */}
        <div className="flex items-center space-x-2">
          <button
            onClick={handlePrevious}
            disabled={!hasPreviousPage}
            className="relative inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed focus:z-20 focus:outline-offset-0"
          >
            <svg
              className="h-5 w-5"
              viewBox="0 0 20 20"
              fill="currentColor"
              aria-hidden="true"
            >
              <path
                fillRule="evenodd"
                d="M12.79 5.23a.75.75 0 01-.02 1.06L8.832 10l3.938 3.71a.75.75 0 11-1.04 1.08l-4.5-4.25a.75.75 0 010-1.08l4.5-4.25a.75.75 0 011.06.02z"
                clipRule="evenodd"
              />
            </svg>
            <span className="ml-1">Previous</span>
          </button>

          {/* Page numbers */}
          <div className="flex items-center space-x-1">
            {/* First page */}
            {currentPage > 2 && (
              <>
                <button
                  onClick={() => onPageChange(1)}
                  className="relative inline-flex items-center rounded-md px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 focus:z-20 focus:outline-offset-0"
                >
                  1
                </button>
                {currentPage > 3 && (
                  <span className="relative inline-flex items-center px-2 py-2 text-sm font-semibold text-gray-700">
                    ...
                  </span>
                )}
              </>
            )}

            {/* Previous page */}
            {hasPreviousPage && (
              <button
                onClick={() => onPageChange(currentPage - 1)}
                className="relative inline-flex items-center rounded-md px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 focus:z-20 focus:outline-offset-0"
              >
                {currentPage - 1}
              </button>
            )}

            {/* Current page */}
            <button
              className="relative z-10 inline-flex items-center rounded-md bg-primary-600 px-3 py-2 text-sm font-semibold text-white focus:z-20 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-primary-600"
              aria-current="page"
            >
              {currentPage}
            </button>

            {/* Next page */}
            {hasNextPage && (
              <button
                onClick={() => onPageChange(currentPage + 1)}
                className="relative inline-flex items-center rounded-md px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 focus:z-20 focus:outline-offset-0"
              >
                {currentPage + 1}
              </button>
            )}

            {/* Last page */}
            {currentPage < totalPages - 1 && (
              <>
                {currentPage < totalPages - 2 && (
                  <span className="relative inline-flex items-center px-2 py-2 text-sm font-semibold text-gray-700">
                    ...
                  </span>
                )}
                <button
                  onClick={() => onPageChange(totalPages)}
                  className="relative inline-flex items-center rounded-md px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 focus:z-20 focus:outline-offset-0"
                >
                  {totalPages}
                </button>
              </>
            )}
          </div>

          <button
            onClick={handleNext}
            disabled={!hasNextPage}
            className="relative inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed focus:z-20 focus:outline-offset-0"
          >
            <span className="mr-1">Next</span>
            <svg
              className="h-5 w-5"
              viewBox="0 0 20 20"
              fill="currentColor"
              aria-hidden="true"
            >
              <path
                fillRule="evenodd"
                d="M7.21 14.77a.75.75 0 01.02-1.06L11.168 10 7.23 6.29a.75.75 0 111.04-1.08l4.5 4.25a.75.75 0 010 1.08l-4.5 4.25a.75.75 0 01-1.06-.02z"
                clipRule="evenodd"
              />
            </svg>
          </button>
        </div>
      </div>
    </div>
  );
};
