import axiosInstance from './axiosConfig';
import { Category, CategoryDto } from './types';

export const categoryApi = {
  getCategories: async () => {
    const response = await axiosInstance.get<Category[]>('/categories');
    return response.data;
  },

  getCategory: async (id: number) => {
    const response = await axiosInstance.get<Category>(`/categories/${id}`);
    return response.data;
  },

  createCategory: async (category: CategoryDto) => {
    const response = await axiosInstance.post<Category>('/categories', category);
    return response.data;
  },

  updateCategory: async (id: number, category: CategoryDto) => {
    const response = await axiosInstance.put<Category>(`/categories/${id}`, category);
    return response.data;
  },

  deleteCategory: async (id: number) => {
    await axiosInstance.delete(`/categories/${id}`);
  }
};
