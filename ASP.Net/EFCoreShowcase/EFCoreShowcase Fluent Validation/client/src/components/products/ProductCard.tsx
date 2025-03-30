import React from 'react';
import { Product } from '../../api/types';
import { Link } from 'react-router-dom';

interface ProductCardProps {
  product: Product;
  onEdit?: (product: Product) => void;
  onDelete?: (id: number) => void;
}

export default function ProductCard({ product, onEdit, onDelete }: ProductCardProps) {
  if (!product) {
    return null;
  }

  return (
    <div className="border rounded-lg p-4 shadow-sm hover:shadow-md transition-shadow">
      <Link to={`/products/${product.id}`}>
        <h3 className="text-lg font-semibold">{product.name}</h3>
      </Link>
      <p className="text-gray-600">${product.price?.toFixed(2) ?? '0.00'}</p>
      <p className="text-gray-500 text-sm truncate">{product.description}</p>
      <p className="text-blue-500 text-sm">
        {product.category?.name ?? 'Uncategorized'}
      </p>
      
      {(onEdit || onDelete) && (
        <div className="mt-4 space-x-2">
          {onEdit && (
            <button
              onClick={() => onEdit(product)}
              className="text-blue-600 hover:text-blue-800"
            >
              Edit
            </button>
          )}
          {onDelete && (
            <button
              onClick={() => onDelete(product.id)}
              className="text-red-600 hover:text-red-800"
            >
              Delete
            </button>
          )}
        </div>
      )}
    </div>
  );
}
