interface LoadingSpinnerProps {
  message?: string;
  size?: "sm" | "md" | "lg";
  className?: string;
}

export const LoadingSpinner = ({
  message = "Loading...",
  size = "md",
  className = "",
}: LoadingSpinnerProps) => {
  const sizeClasses = {
    sm: "h-4 w-4",
    md: "h-12 w-12",
    lg: "h-16 w-16",
  };

  const spinner = (
    <div
      className={`animate-spin rounded-full border-b-2 border-primary-600 ${sizeClasses[size]} ${className}`}
    ></div>
  );

  if (size === "sm") {
    return spinner;
  }

  return (
    <div className="flex flex-col items-center justify-center p-8">
      {spinner}
      <p className="mt-4 text-gray-600">{message}</p>
    </div>
  );
};
