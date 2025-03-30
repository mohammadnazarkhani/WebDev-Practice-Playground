import React from 'react';

interface SearchFormProps {
  searchTerm: string;
  minPrice: string;
  maxPrice: string;
  categoryId: string;
  onSearch: () => void;
  onSearchChange: (field: string, value: string) => void;
  loading: boolean;
}

export default function SearchForm({ 
  searchTerm, 
  minPrice, 
  maxPrice, 
  categoryId,
  onSearch, 
  onSearchChange,
  loading 
}: SearchFormProps) {
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSearch();
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4 mb-6">
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <div>
          <label className="block text-sm font-medium text-gray-700">Search Term</label>
          <input
            type="text"
            value={searchTerm}
            onChange={(e) => onSearchChange('searchTerm', e.target.value)}
            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2"
            placeholder="Search products..."
          />
        </div>
        
        <div>
          <label className="block text-sm font-medium text-gray-700">Min Price</label>
          <input
            type="number"
            value={minPrice}
            onChange={(e) => onSearchChange('minPrice', e.target.value)}
            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2"
            min="0"
            step="0.01"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">Max Price</label>
          <input
            type="number"
            value={maxPrice}
            onChange={(e) => onSearchChange('maxPrice', e.target.value)}
            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2"
            min="0"
            step="0.01"
          />
        </div>

        <div className="flex items-end">
          <button
            type="submit"
            disabled={loading}
            className="w-full bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:bg-blue-300"
          >
            {loading ? 'Searching...' : 'Search'}
          </button>
        </div>
      </div>
    </form>
  );
}
