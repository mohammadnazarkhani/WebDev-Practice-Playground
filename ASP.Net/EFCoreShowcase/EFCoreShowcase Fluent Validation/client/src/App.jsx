import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import ProductList from "./pages/products/ProductList";
import ProductDetail from "./pages/products/ProductDetail";
import CategoryList from "./pages/categories/CategoryList";
import SearchPage from "./pages/search/SearchPage";
import CategoryDetail from "./pages/categories/CategoryDetail";
import EmailPage from "./pages/email/EmailPage";
import { useState, useEffect } from "react";

function App() {
  useEffect(() => {
    // Silence the MetaMask error if Web3 isn't needed
    if (window.ethereum) {
      window.ethereum.autoRefreshOnNetworkChange = false;
    }
  }, []);

  const [apiError, setApiError] = useState(false);

  useEffect(() => {
    // Check if API is available
    const checkApi = async () => {
      try {
        const response = await fetch("http://localhost:5000/api/health");
        if (!response.ok) throw new Error("API responded with non-200 status");
        const data = await response.json();
        if (data.status === "healthy") {
          setApiError(false);
        } else {
          throw new Error("API reported unhealthy status");
        }
      } catch (error) {
        setApiError(true);
        console.error("API server is not available:", error);
      }
    };

    checkApi();
    // Check API status every 30 seconds
    const interval = setInterval(checkApi, 30000);
    return () => clearInterval(interval);
  }, []);

  if (apiError) {
    return (
      <div className="container mx-auto p-4">
        <div
          className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative"
          role="alert"
        >
          <strong className="font-bold">Error!</strong>
          <span className="block sm:inline">
            {" "}
            Cannot connect to the API server. Please make sure it's running on
            http://localhost:5000
          </span>
        </div>
      </div>
    );
  }

  return (
    <Router>
      <div>
        <nav className="bg-gray-800 text-white p-4">
          <div className="container mx-auto">
            <ul className="flex space-x-4">
              <li>
                <Link to="/" className="hover:text-gray-300">
                  Products
                </Link>
              </li>
              <li>
                <Link to="/categories" className="hover:text-gray-300">
                  Categories
                </Link>
              </li>
              <li>
                <Link to="/search" className="hover:text-gray-300">
                  Search
                </Link>
              </li>
              <li>
                <Link to="/email" className="hover:text-gray-300">
                  Email
                </Link>
              </li>
            </ul>
          </div>
        </nav>

        <Routes>
          <Route path="/" element={<ProductList />} />
          <Route path="/products/:id" element={<ProductDetail />} />
          <Route path="/categories" element={<CategoryList />} />
          <Route path="/categories/:id" element={<CategoryDetail />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/email" element={<EmailPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
