export interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  categoryId: number;
  category?: Category;  // Make category optional
}

export interface Category {
  id: number;
  name: string;
}

export interface ProductDto {
  name: string;
  price: number;
  description: string;
  categoryId: number;
}

export interface CategoryDto {
  name: string;
}

export interface ProductSearchParameters {
  searchTerm?: string;
  minPrice?: number;
  maxPrice?: number;
  categoryId?: number;
  page: number;
  pageSize: number;
}

export interface PagedResponse<T> {
  items: T[];
  totalItems: number;
  pageNumber: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface Email {
  address: string;
  subject: string;
  message: string;
}

export interface ValidationRuleDetail {
  name: string;
  expression?: string;
  max?: number;
}

export interface ValidationProperty {
  validators: ValidationRuleDetail[];
}

export interface ValidationProperties {
  address: ValidationProperty;
  subject: ValidationProperty;
  message: ValidationProperty;
}

export interface ValidationRulesResponse {
  properties: ValidationProperties;
}
