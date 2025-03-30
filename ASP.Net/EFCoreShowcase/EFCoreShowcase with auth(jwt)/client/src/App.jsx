import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { AuthProvider, useAuth } from "./utils/authContext";
import Layout from "./components/layout/Layout";
import LoginPage from "./pages/auth/LoginPage";
import RegisterPage from "./pages/auth/RegisterPage";
import ProductsListPage from "./pages/products/ProductsListPage";
import CreateProductPage from "./pages/products/CreateProductPage";
import EditProductPage from "./pages/products/EditProductPage";
import CategoriesListPage from "./pages/categories/CategoriesListPage";
import MyProductsPage from "./pages/products/MyProductsPage";

const ProtectedRoute = ({ children }) => {
  const { isAuthenticated } = useAuth();
  if (!isAuthenticated) return <Navigate to="/login" />;
  return children;
};

function App() {
  return (
    <AuthProvider>
      <Router>
        <Layout>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route
              path="/products"
              element={
                <ProtectedRoute>
                  <ProductsListPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/products/create"
              element={
                <ProtectedRoute>
                  <CreateProductPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/products/edit/:id"
              element={
                <ProtectedRoute>
                  <EditProductPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/products/my"
              element={
                <ProtectedRoute>
                  <MyProductsPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/categories"
              element={
                <ProtectedRoute>
                  <CategoriesListPage />
                </ProtectedRoute>
              }
            />
            <Route path="/" element={<Navigate to="/products" />} />
          </Routes>
        </Layout>
      </Router>
    </AuthProvider>
  );
}

export default App;
