import { Link } from "react-router-dom";
import { useAuth } from "../../utils/authContext";
import { FaList, FaPlus, FaSignOutAlt, FaSignInAlt, FaLayerGroup, FaUser } from "react-icons/fa";

export default function Navbar() {
  const { isAuthenticated, logout, user } = useAuth();

  return (
    <nav className="bg-gray-800">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          <div className="flex items-center">
            <Link to="/" className="text-white font-bold text-xl">
              EF Core Demo
            </Link>
            {isAuthenticated && (
              <div className="ml-10 flex items-baseline space-x-4">
                <Link
                  to="/products"
                  className="flex items-center space-x-2 text-gray-300 hover:text-white px-3 py-2 rounded-md"
                >
                  <FaList className="w-5 h-5" />
                  <span>Products</span>
                </Link>
                <Link
                  to="/products/create"
                  className="flex items-center space-x-2 text-gray-300 hover:text-white px-3 py-2 rounded-md"
                >
                  <FaPlus className="w-5 h-5" />
                  <span>Add Product</span>
                </Link>
                <Link
                  to="/categories"
                  className="flex items-center space-x-2 text-gray-300 hover:text-white px-3 py-2 rounded-md"
                >
                  <FaLayerGroup className="w-5 h-5" />
                  <span>Categories</span>
                </Link>
                <Link
                  to="/products/my"
                  className="flex items-center space-x-2 text-gray-300 hover:text-white px-3 py-2 rounded-md"
                >
                  <FaUser className="w-5 h-5" />
                  <span>My Products</span>
                </Link>
              </div>
            )}
          </div>
          <div className="flex items-center">
            {isAuthenticated ? (
              <div className="flex items-center space-x-4">
                <span className="text-gray-300">Welcome, {user?.username}</span>
                <button
                  onClick={logout}
                  className="flex items-center space-x-2 text-gray-300 hover:text-white px-3 py-2 rounded-md"
                >
                  <FaSignOutAlt className="w-5 h-5" />
                  <span>Logout</span>
                </button>
              </div>
            ) : (
              <Link
                to="/login"
                className="flex items-center space-x-2 text-gray-300 hover:text-white px-3 py-2 rounded-md"
              >
                <FaSignInAlt className="w-5 h-5" />
                <span>Login</span>
              </Link>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}
