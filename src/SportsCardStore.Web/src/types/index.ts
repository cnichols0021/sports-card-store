export enum Category {
  Baseball = 1,
  Football = 2,
  Basketball = 3,
  Hockey = 4,
}

export enum GradingCompany {
  Raw = 0, // Ungraded card
  PSA = 1, // Professional Sports Authenticator
  BGS = 2, // Beckett Grading Services
  SGC = 3, // Sportscard Guaranty Corporation
}

export interface SportsCardResponse {
  id: number;
  playerName: string;
  year: number;
  brand: string;
  setName: string;
  cardNumber: string;
  sport: Category;
  team: string;
  isRookie: boolean;
  isAutograph: boolean;
  isRelic: boolean;
  isBowmanFirst: boolean;
  parallelName?: string;
  printRun?: number;
  grade?: number;
  gradingCompany: GradingCompany;
  condition?: string;
  price: number;
  quantity: number;
  imageUrl?: string;
  description?: string;
  isAvailable: boolean;
  createdDate: string;
  updatedDate: string;
}

export interface PagedSportsCardResponse {
  items: SportsCardResponse[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface CardFilters {
  sport?: Category;
  brand?: string;
  playerName?: string;
  isBowmanFirst?: boolean;
  isAutograph?: boolean;
  minPrice?: number;
  maxPrice?: number;
  parallelName?: string;
  maxPrintRun?: number;
  page?: number;
  pageSize?: number;
}
