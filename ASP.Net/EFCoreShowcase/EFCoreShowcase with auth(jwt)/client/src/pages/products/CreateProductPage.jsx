import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { productsApi } from "../../services/api";
import ProductForm from "../../components/products/ProductForm";

export default function CreateProductPage() {
  const [product, setProduct] = useState({
    name: "",
    price: 0,
    categoryId: "",
  });
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (!product.name.trim()) {
        setError("Product name is required");
        return;
      }
      if (product.price <= 0) {
        setError("Price must be greater than 0");
        return;
      }
      if (!product.categoryId) {
        setError("Category is required");
        return;
      }

      const productData = {
        name: product.name,
        price: parseFloat(product.price),
        categoryId: parseInt(product.categoryId),
      };

      await productsApi.create(productData);
      navigate("/products");
    } catch (error) {
      setError(error.message || "Failed to create product. Please try again.");
      console.error("Failed to create product:", error);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setProduct((prev) => ({ ...prev, [name]: value }));
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-6">Create New Product</h1>
      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}
      <ProductForm
        product={product}
        onSubmit={handleSubmit}
        onChange={handleChange}
        submitLabel="Create Product"
      />
    </div>
  );
}
