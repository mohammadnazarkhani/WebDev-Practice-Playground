import { useState, useEffect } from 'react';
import { Product, ProductDto } from '../../api/types';
import { productApi } from '../../api';
import ProductCard from '../../components/products/ProductCard';
import ProductForm from '../../components/products/ProductForm';
import React from 'react';
import { ErrorBoundary } from '../../components/common/ErrorBoundary';

export default function ProductList() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      const response = await productApi.searchProducts({
        page: 1,
        pageSize: 10
      });
      setProducts(response.items);
    } catch (error) {
      setError('Failed to load products');
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = async (productDto: ProductDto) => {
    try {
      const newProduct = await productApi.createProduct(productDto);
      setProducts([...products, newProduct]);
      setShowForm(false);
    } catch (error) {
      setError('Failed to create product');
    }
  };

  if (loading) return <div>Loading...</div>;

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Products</h1>
        <button 
          className="bg-blue-500 text-white px-4 py-2 rounded"
          onClick={() => setShowForm(true)}
        >
          Add Product
        </button>
      </div>

      {error && <div className="text-red-500 mb-4">{error}</div>}

      {showForm && (
        <div className="mb-4">
          <ProductForm 
            onSubmit={handleCreate}
            onCancel={() => setShowForm(false)}
          />
        </div>
      )}

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {products.map(product => (
          <ErrorBoundary key={product.id}>
            <ProductCard key={product.id} product={product} />
          </ErrorBoundary>
        ))}
      </div>
    </div>
  );
}
