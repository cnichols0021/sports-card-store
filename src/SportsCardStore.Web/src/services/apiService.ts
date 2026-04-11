import {
  PagedSportsCardResponse,
  SportsCardResponse,
  CardFilters,
} from "../types";

const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || "http://localhost:5000";

export class ApiService {
  private baseUrl: string;

  constructor() {
    this.baseUrl = API_BASE_URL;
  }

  async getSportsCards(
    filters?: CardFilters,
  ): Promise<PagedSportsCardResponse> {
    const url = new URL(`${this.baseUrl}/api/sportscards`);

    if (filters) {
      if (filters.sport !== undefined) {
        url.searchParams.append("sport", filters.sport.toString());
      }
      if (filters.brand) {
        url.searchParams.append("brand", filters.brand);
      }
      if (filters.playerName) {
        url.searchParams.append("playerName", filters.playerName);
      }
      if (filters.isBowmanFirst !== undefined) {
        url.searchParams.append(
          "isBowmanFirst",
          filters.isBowmanFirst.toString(),
        );
      }
      if (filters.isAutograph !== undefined) {
        url.searchParams.append(
          "isAutograph",
          filters.isAutograph.toString(),
        );
      }
      if (filters.minPrice !== undefined) {
        url.searchParams.append("minPrice", filters.minPrice.toString());
      }
      if (filters.maxPrice !== undefined) {
        url.searchParams.append("maxPrice", filters.maxPrice.toString());
      }
      if (filters.parallelName) {
        url.searchParams.append("parallelName", filters.parallelName);
      }
      if (filters.maxPrintRun !== undefined) {
        url.searchParams.append("maxPrintRun", filters.maxPrintRun.toString());
      }
      if (filters.page !== undefined) {
        url.searchParams.append("page", filters.page.toString());
      }
      if (filters.pageSize !== undefined) {
        url.searchParams.append("pageSize", filters.pageSize.toString());
      }
    }

    const response = await fetch(url.toString());

    if (!response.ok) {
      throw new Error(`Failed to fetch sports cards: ${response.statusText}`);
    }

    return await response.json();
  }

  async getSportsCard(id: number): Promise<SportsCardResponse> {
    const response = await fetch(`${this.baseUrl}/api/sportscards/${id}`);

    if (!response.ok) {
      if (response.status === 404) {
        throw new Error("Sports card not found");
      }
      throw new Error(`Failed to fetch sports card: ${response.statusText}`);
    }

    return await response.json();
  }

  async uploadCardImage(
    id: number,
    file: File,
  ): Promise<SportsCardResponse> {
    const formData = new FormData();
    formData.append("image", file);

    const response = await fetch(
      `${this.baseUrl}/api/sportscards/${id}/image`,
      {
        method: "POST",
        body: formData,
      },
    );

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || `Upload failed: ${response.statusText}`);
    }

    return await response.json();
  }

  async deleteCardImage(id: number): Promise<void> {
    const response = await fetch(
      `${this.baseUrl}/api/sportscards/${id}/image`,
      {
        method: "DELETE",
      },
    );

    if (!response.ok) {
      throw new Error(`Failed to delete image: ${response.statusText}`);
    }
  }
}

export const apiService = new ApiService();
