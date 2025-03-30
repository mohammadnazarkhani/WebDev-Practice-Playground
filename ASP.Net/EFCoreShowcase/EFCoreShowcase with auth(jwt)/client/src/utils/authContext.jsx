import { createContext, useContext, useState } from "react";
import { authApi } from "../services/api";

// Initialize with default values instead of null
const AuthContext = createContext({
  user: null,
  isAuthenticated: false,
  login: async () => {},
  logout: () => {},
  register: async () => {},
});

export const AuthProvider = ({ children }) => {
  // Add try-catch for safer JSON parsing
  const [user, setUser] = useState(() => {
    try {
      return JSON.parse(localStorage.getItem("user")) || null;
    } catch {
      return null;
    }
  });

  const [isAuthenticated, setIsAuthenticated] = useState(
    !!localStorage.getItem("token")
  );

  const login = async (credentials) => {
    const response = await authApi.login(credentials);
    localStorage.setItem("token", response.token);
    localStorage.setItem(
      "user",
      JSON.stringify({ username: response.username })
    );
    setUser({ username: response.username });
    setIsAuthenticated(true);
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
    setIsAuthenticated(false);
  };

  const register = async (userData) => {
    const response = await authApi.register(userData);
    localStorage.setItem("token", response.token);
    localStorage.setItem(
      "user",
      JSON.stringify({ username: response.username })
    );
    setUser({ username: response.username });
    setIsAuthenticated(true);
  };

  const contextValue = {
    user,
    isAuthenticated,
    login,
    logout,
    register,
  };

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
