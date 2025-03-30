import React, { useState, useEffect } from 'react';
import { Product, ProductDto, Category } from '../../api/types';
import { categoryApi } from '../../api';

interface ProductFormProps {
  onSubmit: (productDto: ProductDto) => Promise<void>;
  onCancel: () => void;
  initialData?: Product;
}

interface ValidationErrors {
  [key: string]: string[];
}

export default function ProductForm({ initialData, onSubmit, onCancel }: ProductFormProps) {
  const [formData, setFormData] = useState<ProductDto>({
    name: '',
    price: 0,
    description: '',
    categoryId: 0
  });
  const [categories, setCategories] = useState<Category[]>([]);
  const [error, setError] = useState('');
  const [validationErrors, setValidationErrors] = useState<ValidationErrors>({});

  useEffect(() => {
    if (initialData) {
      setFormData({
        name: initialData.name,
        price: initialData.price,
        description: initialData.description,
        categoryId: initialData.categoryId
      });
    }
    loadCategories();
  }, [initialData]);

  const loadCategories = async () => {
    try {
      const data = await categoryApi.getCategories();
      setCategories(data);
    } catch (err) {
      setError('Failed to load categories');
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setValidationErrors({});
    
    try {
      await onSubmit(formData);
    } catch (err: any) {
      if (err.name === 'ValidationError') {
        setValidationErrors(err.errors.reduce((acc: ValidationErrors, curr: any) => {
          acc[curr.property] = [...(acc[curr.property] || []), curr.error];
          return acc;
        }, {}));
      } else {
        setError('Failed to save product');
      }
    }
  };

  const getFieldError = (fieldName: string) => {
    return validationErrors[fieldName]?.join(', ');
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label htmlFor="name" className="block text-sm font-medium text-gray-700">
          Product Name
        </label>
        <input
          type="text"
          id="name"
          value={formData.name}
          onChange={(e) => setFormData({ ...formData, name: e.target.value })}
          className={`mt-1 block w-full rounded-md border ${
            getFieldError('name') ? 'border-red-500' : 'border-gray-300'
          } px-3 py-2`}
          required
        />
        {getFieldError('name') && (
          <p className="mt-1 text-sm text-red-500">{getFieldError('name')}</p>
        )}
      </div>

      <div>
        <label htmlFor="price" className="block text-sm font-medium text-gray-700">
          Price
        </label>
        <input
          type="number"
          id="price"
          value={formData.price}
          onChange={(e) => setFormData({ ...formData, price: Number(e.target.value) })}
          className={`mt-1 block w-full rounded-md border ${
            getFieldError('price') ? 'border-red-500' : 'border-gray-300'
          } px-3 py-2`}
          required
          min="0"
          step="0.01"
        />
        {getFieldError('price') && (
          <p className="mt-1 text-sm text-red-500">{getFieldError('price')}</p>
        )}
      </div>

      <div>
        <label htmlFor="description" className="block text-sm font-medium text-gray-700">
          Description
        </label>
        <textarea
          id="description"
          value={formData.description}
          onChange={(e) => setFormData({ ...formData, description: e.target.value })}
          className={`mt-1 block w-full rounded-md border ${
            getFieldError('description') ? 'border-red-500' : 'border-gray-300'
          } px-3 py-2`}
          rows={3}
        />
        {getFieldError('description') && (
          <p className="mt-1 text-sm text-red-500">{getFieldError('description')}</p>
        )}
      </div>

      <div>
        <label htmlFor="category" className="block text-sm font-medium text-gray-700">
          Category
        </label>
        <select
          id="category"
          value={formData.categoryId}
          onChange={(e) => setFormData({ ...formData, categoryId: Number(e.target.value) })}
          className={`mt-1 block w-full rounded-md border ${
            getFieldError('categoryId') ? 'border-red-500' : 'border-gray-300'
          } px-3 py-2`}
          required
        >
          <option value="">Select a category</option>
          {categories.map(category => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
        {getFieldError('categoryId') && (
          <p className="mt-1 text-sm text-red-500">{getFieldError('categoryId')}</p>
        )}
      </div>

      {error && <p className="text-red-600 text-sm">{error}</p>}

      <div className="flex space-x-2">
        <button
          type="submit"
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          {initialData ? 'Update' : 'Create'}
        </button>
        <button
          type="button"
          onClick={onCancel}
          className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400"
        >
          Cancel
        </button>
      </div>
    </form>
  );
}
