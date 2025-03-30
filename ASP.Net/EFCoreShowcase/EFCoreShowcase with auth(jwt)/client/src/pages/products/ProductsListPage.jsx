import { useState, useEffect } from "react";
import { productsApi } from "../../services/api";
import ProductsTable from "../../components/products/ProductsTable";

export default function ProductsListPage() {
  const [products, setProducts] = useState([]);
  const [error, setError] = useState("");

  const fetchProducts = async () => {
    try {
      const data = await productsApi.getAll();
      setProducts(data);
      setError(""); // Clear any previous errors
    } catch (error) {
      setError(error.message || "Failed to fetch products");
      console.error("Error fetching products:", error);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Products</h1>
      </div>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      <ProductsTable products={products} readOnly={true} />
    </div>
  );
}
