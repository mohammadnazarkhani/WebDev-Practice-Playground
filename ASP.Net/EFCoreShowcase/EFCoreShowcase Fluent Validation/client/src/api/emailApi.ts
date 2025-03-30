import axiosInstance from './axiosConfig';
import { Email, ValidationRulesResponse } from './types';

export const emailApi = {
  sendEmail: async (email: Email) => {
    const response = await axiosInstance.post<{ message: string }>('/email', email);
    return response.data;
  },

  getValidationRules: async () => {
    const response = await axiosInstance.get<ValidationRulesResponse>('/email/validation-rules');
    return response.data;
  }
};
