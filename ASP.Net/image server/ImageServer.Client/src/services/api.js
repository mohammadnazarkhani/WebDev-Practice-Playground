import axios from "axios";

const BASE_URL = "https://localhost:7000";

const api = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const uploadImage = async (formData) => {
  const response = await api.post("/upload", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return response.data;
};

export const getImages = async () => {
  const response = await api.get("/images");
  return response.data;
};

export const deleteImage = async (id) => {
  const response = await api.delete(`/images/${id}`);
  return response.data;
};

export const updateImage = async (id, formData) => {
  const response = await api.put(`/images/${id}`, formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return response.data;
};

export const patchImage = async (id, formData) => {
  const response = await api.patch(`/images/${id}`, formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return response.data;
};

export const getImageDetails = async (id) => {
  const response = await api.get(`/images/${id}/details`);
  return response.data;
};

export const deleteAllImages = async () => {
  const response = await api.delete("/images/all");
  return response.data;
};

export default api;
