import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { apiService } from "../../services/apiService";
import { CreateSportsCardRequest, Category } from "../../types/index";
import { LoadingSpinner } from "../../components/LoadingSpinner";
import { ErrorMessage } from "../../components/ErrorMessage";

export function CardFormPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const isEditing = Boolean(id);

  const [formData, setFormData] = useState<CreateSportsCardRequest>({
    playerName: "",
    year: new Date().getFullYear(),
    brand: "",
    setName: "",
    cardNumber: "",
    sport: Category.Baseball,
    team: "",
    isRookie: false,
    isAutograph: false,
    isRelic: false,
    isBowmanFirst: false,
    parallelName: "",
    printRun: undefined,
    gradingCompany: "",
    grade: undefined,
    condition: "Near Mint or Better",
    price: 0,
    quantity: 1,
    description: "",
    isAvailable: true,
  });

  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingCard, setIsLoadingCard] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  useEffect(() => {
    if (isEditing && id) {
      const fetchCard = async () => {
        try {
          setIsLoadingCard(true);
          setError(null);
          const card = await apiService.getSportsCard(parseInt(id));

          setFormData({
            playerName: card.playerName,
            year: card.year,
            brand: card.brand,
            setName: card.setName,
            cardNumber: card.cardNumber,
            sport: card.sport,
            team: card.team,
            isRookie: card.isRookie,
            isAutograph: card.isAutograph,
            isRelic: card.isRelic,
            isBowmanFirst: card.isBowmanFirst,
            parallelName: card.parallelName || "",
            printRun: card.printRun,
            gradingCompany: card.gradingCompany || "",
            grade: card.grade,
            condition: card.condition || "Near Mint or Better",
            price: card.price,
            quantity: card.quantity,
            description: card.description || "",
            isAvailable: card.isAvailable,
          });
        } catch (err) {
          setError(err instanceof Error ? err.message : "Failed to load card");
        } finally {
          setIsLoadingCard(false);
        }
      };

      fetchCard();
    }
  }, [isEditing, id]);

  const handleInputChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
    >,
  ) => {
    const { name, value, type } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]:
        type === "checkbox"
          ? (e.target as HTMLInputElement).checked
          : type === "number"
            ? value === ""
              ? undefined
              : parseFloat(value)
            : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      setIsLoading(true);
      setError(null);
      setSuccessMessage(null);

      if (isEditing && id) {
        await apiService.updateSportsCard(parseInt(id), formData);
        setSuccessMessage("Card updated successfully!");
      } else {
        await apiService.createSportsCard(formData);
        setSuccessMessage("Card created successfully!");
      }

      setTimeout(() => {
        navigate("/admin/cards");
      }, 1500);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to save card");
    } finally {
      setIsLoading(false);
    }
  };

  const handleCancel = () => {
    navigate("/admin/cards");
  };

  if (isLoadingCard) {
    return (
      <div className="p-6">
        <LoadingSpinner />
      </div>
    );
  }

  return (
    <div className="p-6">
      <div className="max-w-4xl mx-auto">
        <h2 className="text-2xl font-bold text-gray-900 mb-8">
          {isEditing ? "Edit Card" : "Create New Card"}
        </h2>

        {error && <ErrorMessage message={error} className="mb-6" />}
        {successMessage && (
          <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded mb-6">
            {successMessage}
          </div>
        )}

        <form
          onSubmit={handleSubmit}
          className="space-y-6 bg-white shadow px-6 py-8 rounded-lg"
        >
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Player Name */}
            <div>
              <label
                htmlFor="playerName"
                className="block text-sm font-medium text-gray-700"
              >
                Player Name *
              </label>
              <input
                type="text"
                id="playerName"
                name="playerName"
                required
                value={formData.playerName}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Year */}
            <div>
              <label
                htmlFor="year"
                className="block text-sm font-medium text-gray-700"
              >
                Year *
              </label>
              <input
                type="number"
                id="year"
                name="year"
                required
                min="1800"
                max="2100"
                value={formData.year}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Brand */}
            <div>
              <label
                htmlFor="brand"
                className="block text-sm font-medium text-gray-700"
              >
                Brand *
              </label>
              <input
                type="text"
                id="brand"
                name="brand"
                required
                value={formData.brand}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Set Name */}
            <div>
              <label
                htmlFor="setName"
                className="block text-sm font-medium text-gray-700"
              >
                Set Name *
              </label>
              <input
                type="text"
                id="setName"
                name="setName"
                required
                value={formData.setName}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Card Number */}
            <div>
              <label
                htmlFor="cardNumber"
                className="block text-sm font-medium text-gray-700"
              >
                Card Number *
              </label>
              <input
                type="text"
                id="cardNumber"
                name="cardNumber"
                required
                value={formData.cardNumber}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Sport */}
            <div>
              <label
                htmlFor="sport"
                className="block text-sm font-medium text-gray-700"
              >
                Sport *
              </label>
              <select
                id="sport"
                name="sport"
                required
                value={formData.sport}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              >
                <option value={Category.Baseball}>Baseball</option>
                <option value={Category.Basketball}>Basketball</option>
                <option value={Category.Football}>Football</option>
                <option value={Category.Hockey}>Hockey</option>
              </select>
            </div>

            {/* Team */}
            <div>
              <label
                htmlFor="team"
                className="block text-sm font-medium text-gray-700"
              >
                Team *
              </label>
              <input
                type="text"
                id="team"
                name="team"
                required
                value={formData.team}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Price */}
            <div>
              <label
                htmlFor="price"
                className="block text-sm font-medium text-gray-700"
              >
                Price *
              </label>
              <input
                type="number"
                id="price"
                name="price"
                required
                min="0"
                step="0.01"
                value={formData.price}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Quantity */}
            <div>
              <label
                htmlFor="quantity"
                className="block text-sm font-medium text-gray-700"
              >
                Quantity *
              </label>
              <input
                type="number"
                id="quantity"
                name="quantity"
                required
                min="0"
                value={formData.quantity}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Condition */}
            <div>
              <label
                htmlFor="condition"
                className="block text-sm font-medium text-gray-700"
              >
                Condition
              </label>
              <input
                type="text"
                id="condition"
                name="condition"
                value={formData.condition}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Parallel Name */}
            <div>
              <label
                htmlFor="parallelName"
                className="block text-sm font-medium text-gray-700"
              >
                Parallel Name
              </label>
              <input
                type="text"
                id="parallelName"
                name="parallelName"
                value={formData.parallelName}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Print Run */}
            <div>
              <label
                htmlFor="printRun"
                className="block text-sm font-medium text-gray-700"
              >
                Print Run
              </label>
              <input
                type="number"
                id="printRun"
                name="printRun"
                min="0"
                value={formData.printRun || ""}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Grading Company */}
            <div>
              <label
                htmlFor="gradingCompany"
                className="block text-sm font-medium text-gray-700"
              >
                Grading Company
              </label>
              <input
                type="text"
                id="gradingCompany"
                name="gradingCompany"
                value={formData.gradingCompany}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            {/* Grade */}
            <div>
              <label
                htmlFor="grade"
                className="block text-sm font-medium text-gray-700"
              >
                Grade
              </label>
              <input
                type="number"
                id="grade"
                name="grade"
                min="0"
                max="10"
                step="0.5"
                value={formData.grade || ""}
                onChange={handleInputChange}
                className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
          </div>

          {/* Checkboxes */}
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
            <div className="flex items-center">
              <input
                type="checkbox"
                id="isRookie"
                name="isRookie"
                checked={formData.isRookie}
                onChange={handleInputChange}
                className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
              />
              <label htmlFor="isRookie" className="ml-2 text-sm text-gray-700">
                Rookie Card
              </label>
            </div>

            <div className="flex items-center">
              <input
                type="checkbox"
                id="isAutograph"
                name="isAutograph"
                checked={formData.isAutograph}
                onChange={handleInputChange}
                className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
              />
              <label
                htmlFor="isAutograph"
                className="ml-2 text-sm text-gray-700"
              >
                Autograph
              </label>
            </div>

            <div className="flex items-center">
              <input
                type="checkbox"
                id="isRelic"
                name="isRelic"
                checked={formData.isRelic}
                onChange={handleInputChange}
                className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
              />
              <label htmlFor="isRelic" className="ml-2 text-sm text-gray-700">
                Relic
              </label>
            </div>

            <div className="flex items-center">
              <input
                type="checkbox"
                id="isBowmanFirst"
                name="isBowmanFirst"
                checked={formData.isBowmanFirst}
                onChange={handleInputChange}
                className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
              />
              <label
                htmlFor="isBowmanFirst"
                className="ml-2 text-sm text-gray-700"
              >
                Bowman First
              </label>
            </div>

            <div className="flex items-center">
              <input
                type="checkbox"
                id="isAvailable"
                name="isAvailable"
                checked={formData.isAvailable}
                onChange={handleInputChange}
                className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
              />
              <label
                htmlFor="isAvailable"
                className="ml-2 text-sm text-gray-700"
              >
                Available
              </label>
            </div>
          </div>

          {/* Description */}
          <div>
            <label
              htmlFor="description"
              className="block text-sm font-medium text-gray-700"
            >
              Description
            </label>
            <textarea
              id="description"
              name="description"
              rows={4}
              value={formData.description}
              onChange={handleInputChange}
              className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
            />
          </div>

          {/* Submit buttons */}
          <div className="flex justify-end space-x-3">
            <button
              type="button"
              onClick={handleCancel}
              className="px-4 py-2 border border-gray-300 rounded-md text-sm font-medium text-gray-700 bg-white hover:bg-gray-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={isLoading}
              className="px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 disabled:opacity-50"
            >
              {isLoading ? (
                <span className="flex items-center">
                  <LoadingSpinner size="sm" className="mr-2" />
                  {isEditing ? "Updating..." : "Creating..."}
                </span>
              ) : isEditing ? (
                "Update Card"
              ) : (
                "Create Card"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
