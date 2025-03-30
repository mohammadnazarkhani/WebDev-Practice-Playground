import { useState, useEffect } from "react";
import { categoriesApi } from "../../services/api";
import CategoriesTable from "../../components/categories/CategoriesTable";

export default function CategoriesListPage() {
  const [categories, setCategories] = useState([]);
  const [newCategory, setNewCategory] = useState("");
  const [error, setError] = useState("");

  const fetchCategories = async () => {
    try {
      const data = await categoriesApi.getAll();
      setCategories(data);
    } catch (error) {
      setError("Failed to fetch categories");
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const created = await categoriesApi.create({ name: newCategory });
      setCategories([...categories, created]);
      setNewCategory("");
    } catch (error) {
      setError("Failed to create category");
    }
  };

  const handleDelete = async (id) => {
    if (
      !window.confirm(
        "Are you sure? This will also delete all related products."
      )
    )
      return;
    try {
      await categoriesApi.delete(id);
      setCategories(categories.filter((c) => c.id !== id));
    } catch (error) {
      setError("Failed to delete category");
    }
  };

  useEffect(() => {
    fetchCategories();
  }, []);

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-6">Categories</h1>

      <form onSubmit={handleSubmit} className="mb-6">
        <div className="flex gap-4">
          <input
            type="text"
            value={newCategory}
            onChange={(e) => setNewCategory(e.target.value)}
            placeholder="Category name"
            required
            className="flex-1 px-3 py-2 border border-gray-300 rounded-md"
          />
          <button
            type="submit"
            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
          >
            Add Category
          </button>
        </div>
      </form>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      <CategoriesTable categories={categories} onDelete={handleDelete} />
    </div>
  );
}
