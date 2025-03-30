import { useState } from 'react';
import { Product, ProductSearchParameters } from '../../api/types';
import { productApi } from '../../api';
import React from 'react';
import ProductCard from '../../components/products/ProductCard';
import SearchForm from '../../components/search/SearchForm';

export default function SearchPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  const [searchParams, setSearchParams] = useState({
    searchTerm: '',
    minPrice: '',
    maxPrice: '',
    categoryId: ''
  });

  const handleSearchChange = (field: string, value: string) => {
    setSearchParams(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const handleSearch = async () => {
    setLoading(true);
    setError('');
    
    try {
      const params: ProductSearchParameters = {
        searchTerm: searchParams.searchTerm,
        minPrice: searchParams.minPrice ? Number(searchParams.minPrice) : undefined,
        maxPrice: searchParams.maxPrice ? Number(searchParams.maxPrice) : undefined,
        categoryId: searchParams.categoryId ? Number(searchParams.categoryId) : undefined,
        page: 1,
        pageSize: 10
      };
      
      const response = await productApi.searchProducts(params);
      setProducts(response.items);
    } catch (error) {
      setError('Search failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Search Products</h1>
      
      <SearchForm
        {...searchParams}
        onSearch={handleSearch}
        onSearchChange={handleSearchChange}
        loading={loading}
      />

      {error && (
        <div className="text-red-500 mb-4">{error}</div>
      )}

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {products.map(product => (
          <ProductCard 
            key={product.id} 
            product={product}
          />
        ))}
      </div>

      {!loading && products.length === 0 && (
        <div className="text-center text-gray-500 mt-8">
          No products found. Try adjusting your search criteria.
        </div>
      )}
    </div>
  );
}
