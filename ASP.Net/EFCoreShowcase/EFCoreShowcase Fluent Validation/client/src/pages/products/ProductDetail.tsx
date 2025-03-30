import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Product } from '../../api/types';
import { productApi } from '../../api';
import ProductForm from '../../components/products/ProductForm';

export default function ProductDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [product, setProduct] = useState<Product | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const loadProduct = async () => {
      if (!id) return;
      try {
        const data = await productApi.getProduct(parseInt(id));
        setProduct(data);
      } catch (error) {
        setError('Failed to load product');
      }
    };

    loadProduct();
  }, [id]);

  const handleUpdate = async (productDto: any) => {
    if (!product) return;
    try {
      const updated = await productApi.updateProduct(product.id, productDto);
      setProduct(updated);
      setIsEditing(false);
    } catch (error) {
      setError('Failed to update product');
    }
  };

  const handleDelete = async () => {
    if (!product || !window.confirm('Are you sure you want to delete this product?')) return;
    try {
      await productApi.deleteProduct(product.id);
      navigate('/');
    } catch (error) {
      setError('Failed to delete product');
    }
  };

  if (!product && !error) return <div>Loading...</div>;
  if (error) return <div className="text-red-500">{error}</div>;

  return (
    <div className="container mx-auto p-4">
      {isEditing ? (
        <ProductForm
          initialData={product!}
          onSubmit={handleUpdate}
          onCancel={() => setIsEditing(false)}
        />
      ) : (
        <div className="bg-white shadow rounded-lg p-6">
          <h1 className="text-2xl font-bold mb-4">{product?.name}</h1>
          <p className="text-xl text-gray-700 mb-2">${product?.price.toFixed(2)}</p>
          <p className="text-gray-600 mb-4">{product?.description}</p>
          <p className="text-blue-500 mb-4">Category: {product?.category?.name ?? 'Uncategorized'}</p>
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
