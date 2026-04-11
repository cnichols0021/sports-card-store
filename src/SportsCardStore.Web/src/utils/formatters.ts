import { Category, GradingCompany } from "../types";

export const formatCategory = (category: Category): string => {
  switch (category) {
    case Category.Baseball:
      return "Baseball";
    case Category.Football:
      return "Football";
    case Category.Basketball:
      return "Basketball";
    case Category.Hockey:
      return "Hockey";
    default:
      return "Unknown";
  }
};

export const formatGradingCompany = (company: GradingCompany): string => {
  switch (company) {
    case GradingCompany.Raw:
      return "Raw";
    case GradingCompany.PSA:
      return "PSA";
    case GradingCompany.BGS:
      return "BGS";
    case GradingCompany.SGC:
      return "SGC";
    default:
      return "Unknown";
  }
};

export const formatPrice = (price: number): string => {
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
  }).format(price);
};

export const formatGrade = (grade?: number): string => {
  return grade ? grade.toString() : "Ungraded";
};
