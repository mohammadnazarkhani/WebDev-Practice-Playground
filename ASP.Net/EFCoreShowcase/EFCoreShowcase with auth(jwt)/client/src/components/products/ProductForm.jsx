import { useEffect, useState } from "react";
import { categoriesApi } from "../../services/api";
import Input from "../forms/Input";

export default function ProductForm({
  product,
  onSubmit,
  onChange,
  submitLabel,
  error,
}) {
  const [categories, setCategories] = useState([]);
  const [loadingCategories, setLoadingCategories] = useState(true);
  const [categoryError, setCategoryError] = useState("");

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data = await categoriesApi.getAll();
        setCategories(data);
      } catch (error) {
        setCategoryError("Failed to load categories");
      } finally {
        setLoadingCategories(false);
      }
    };
    fetchCategories();
  }, []);

  return (
    <form onSubmit={onSubmit} className="max-w-md space-y-4">
      <Input
        label="Product Name"
        name="name"
        value={product.name}
        onChange={onChange}
        required
      />
      <Input
        label="Price"
        name="price"
        type="number"
        min="0"
        step="0.01"
        value={product.price}
        onChange={onChange}
        required
      />
      <div className="space-y-1">
        <label className="block text-sm font-medium text-gray-700">
          Category
        </label>
        {loadingCategories ? (
          <div className="text-gray-500">Loading categories...</div>
        ) : categoryError ? (
          <div className="text-red-600">{categoryError}</div>
        ) : (
          <select
            name="categoryId"
            value={product.categoryId || ""}
            onChange={onChange}
            required
            className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
          >
            <option value="">Select a category</option>
            {categories.map((category) => (
              <option key={category.id} value={category.id}>
                {category.name}
              </option>
            ))}
          </select>
        )}
      </div>
      <button
        type="submit"
        className="w-full py-2 px-4 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
      >
        {submitLabel}
      </button>
    </form>
  );
}
