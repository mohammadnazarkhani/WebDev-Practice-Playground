import { useState, useEffect } from "react";
import { productsApi } from "../../services/api";
import ProductsTable from "../../components/products/ProductsTable";

export default function MyProductsPage() {
  const [products, setProducts] = useState([]);
  const [error, setError] = useState("");

  const fetchProducts = async () => {
    try {
      const data = await productsApi.getUserProducts();
      setProducts(data);
    } catch (error) {
      setError("Failed to fetch your products");
      console.error(error);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Are you sure you want to delete this product?"))
      return;
    try {
      await productsApi.delete(id);
      setProducts(products.filter((p) => p.id !== id));
    } catch (error) {
      setError("Failed to delete product");
      console.error(error);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-6">My Products</h1>
      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}
      <ProductsTable products={products} onDelete={handleDelete} />
    </div>
  );
}
