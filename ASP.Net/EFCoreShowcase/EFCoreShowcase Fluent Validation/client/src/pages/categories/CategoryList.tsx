import { useState, useEffect } from 'react';
import { Category, CategoryDto } from '../../api/types';
import { categoryApi } from '../../api';
import CategoryCard from '../../components/categories/CategoryCard';
import CategoryForm from '../../components/categories/CategoryForm';
import React from 'react';

export default function CategoryList() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = useState(false);
  const [editingCategory, setEditingCategory] = useState<Category | undefined>(undefined);
  const [error, setError] = useState('');

  useEffect(() => {
    const loadCategories = async () => {
      try {
        const response = await categoryApi.getCategories();
        setCategories(response);
      } catch (error) {
        console.error('Failed to load categories:', error);
        setError('Failed to load categories');
      } finally {
        setLoading(false);
      }
    };

    loadCategories();
  }, []);

  const handleCreate = async (categoryDto: CategoryDto) => {
    try {
      const result = await categoryApi.createCategory(categoryDto);
      setCategories([...categories, result]);
      setShowForm(false);
      setError('');
    } catch (error) {
      console.error('Failed to create category:', error);
      setError('Failed to create category');
    }
  };

  const handleUpdate = async (categoryDto: CategoryDto) => {
    if (!editingCategory) return;

    try {
      const updatedCategory = await categoryApi.updateCategory(editingCategory.id, categoryDto);
      setCategories(categories.map(cat => 
        cat.id === updatedCategory.id ? updatedCategory : cat
      ));
      setEditingCategory(undefined);
      setShowForm(false);
      setError('');
    } catch (error) {
      console.error('Failed to update category:', error);
      setError('Failed to update category');
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Are you sure you want to delete this category?')) return;

    try {
      await categoryApi.deleteCategory(id);
      setCategories(categories.filter(cat => cat.id !== id));
      setError('');
    } catch (error) {
      console.error('Failed to delete category:', error);
      setError('Failed to delete category');
    }
  };

  if (loading) return <div>Loading...</div>;

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Categories</h1>
      {error && <div className="text-red-500 mb-4">{error}</div>}
      <button 
        className="bg-blue-500 text-white px-4 py-2 rounded mb-4"
        onClick={() => setShowForm(true)}
      >
        Add Category
      </button>
      {showForm && (
        <CategoryForm 
          onSubmit={editingCategory ? handleUpdate : handleCreate} 
          onCancel={() => setShowForm(false)} 
          initialData={editingCategory}
        />
      )}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {categories.map(category => (
          <CategoryCard 
            key={category.id} 
            category={category} 
            onEdit={() => {
              setEditingCategory(category);
              setShowForm(true);
            }} 
            onDelete={() => handleDelete(category.id)} 
          />
        ))}
      </div>
    </div>
  );
}
