const API_URL = "http://localhost:5236/api";

// Auth API methods
export const authApi = {
  login: async (credentials) => {
    const response = await fetch(`${API_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(credentials),
    });
    if (!response.ok) throw new Error("Login failed");
    return response.json();
  },

  register: async (userData) => {
    const response = await fetch(`${API_URL}/auth/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(userData),
    });

    if (response.status === 409) {
      throw new Error(
        "Username is already taken. Please choose a different one."
      );
    }

    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || "Registration failed");
    }

    return response.json();
  },
};

// Products API methods
export const productsApi = {
  getAll: async () => {
    const response = await fetch(`${API_URL}/products`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || "Failed to fetch products");
    }
    return response.json();
  },

  getById: async (id) => {
    const response = await fetch(`${API_URL}/products/${id}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    if (!response.ok) throw new Error("Failed to fetch product");
    return response.json();
  },

  create: async (product) => {
    const response = await fetch(`${API_URL}/products`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify(product),
    });
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || "Failed to create product");
    }
    return response.json();
  },

  update: async (id, product) => {
    const response = await fetch(`${API_URL}/products/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify(product),
    });
    if (!response.ok) throw new Error("Failed to update product");
    return response.json();
  },

  delete: async (id) => {
    const response = await fetch(`${API_URL}/products/${id}`, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    if (!response.ok) throw new Error("Failed to delete product");
    return true;
  },

  getUserProducts: async () => {
    const response = await fetch(`${API_URL}/products/my`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    if (!response.ok) throw new Error("Failed to fetch user products");
    return response.json();
  },
};

// Categories API methods
export const categoriesApi = {
  getAll: async () => {
    const response = await fetch(`${API_URL}/categories`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    if (!response.ok) throw new Error("Failed to fetch categories");
    return response.json();
  },

  create: async (category) => {
    const response = await fetch(`${API_URL}/categories`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify(category),
    });
    if (!response.ok) throw new Error("Failed to create category");
    return response.json();
  },

  delete: async (id) => {
    const response = await fetch(`${API_URL}/categories/${id}`, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    if (!response.ok) throw new Error("Failed to delete category");
    return true;
  },
};
