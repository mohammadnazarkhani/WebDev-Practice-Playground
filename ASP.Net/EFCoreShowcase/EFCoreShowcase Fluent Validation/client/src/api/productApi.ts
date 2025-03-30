import axiosInstance from './axiosConfig';
import { Product, ProductDto, ProductSearchParameters, PagedResponse } from './types';

export const productApi = {
  searchProducts: async (params: ProductSearchParameters) => {
    const response = await axiosInstance.get<PagedResponse<Product>>('/products', { params });
    return response.data;
  },

  getProduct: async (id: number) => {
    const response = await axiosInstance.get<Product>(`/products/${id}`);
    return response.data;
  },

  createProduct: async (product: ProductDto) => {
    const response = await axiosInstance.post<Product>('/products', product);
    return response.data;
  },

  updateProduct: async (id: number, product: ProductDto) => {
    const response = await axiosInstance.put<Product>(`/products/${id}`, product);
    return response.data;
  },

  deleteProduct: async (id: number) => {
    await axiosInstance.delete(`/products/${id}`);
  }
};
