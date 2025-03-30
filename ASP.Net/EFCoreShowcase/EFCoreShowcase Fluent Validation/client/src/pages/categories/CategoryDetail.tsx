import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Category } from '../../api/types';
import { categoryApi } from '../../api';
import CategoryForm from '../../components/categories/CategoryForm';

export default function CategoryDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [category, setCategory] = useState<Category | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const loadCategory = async () => {
      if (!id) return;
      try {
        const data = await categoryApi.getCategory(parseInt(id));
        setCategory(data);
      } catch (error) {
        console.error('Failed to load category:', error);
        setError('Failed to load category');
      }
    };

    loadCategory();
  }, [id]);

  const handleUpdate = async (categoryDto: { name: string }) => {
    if (!category) return;
    try {
      const updated = await categoryApi.updateCategory(category.id, categoryDto);
      setCategory(updated);
      setIsEditing(false);
    } catch (error) {
      console.error('Failed to update category:', error);
      setError('Failed to update category');
    }
  };

  const handleDelete = async () => {
    if (!category || !window.confirm('Are you sure you want to delete this category?')) return;
    try {
      await categoryApi.deleteCategory(category.id);
      navigate('/categories');
    } catch (error) {
      console.error('Failed to delete category:', error);
      setError('Failed to delete category');
    }
  };

  if (!category) return <div>Loading...</div>;
  if (error) return <div className="text-red-500">{error}</div>;

  return (
    <div className="container mx-auto p-4">
      {isEditing ? (
        <CategoryForm
          initialData={category}
          onSubmit={handleUpdate}
          onCancel={() => setIsEditing(false)}
        />
      ) : (
        <div className="bg-white shadow rounded-lg p-6">
          <h1 className="text-2xl font-bold mb-4">{category.name}</h1>
          <div className="space-x-4">
            <button
              onClick={() => setIsEditing(true)}
              className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
            >
              Edit
            </button>
            <button
              onClick={handleDelete}
              className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
            >
              Delete
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
