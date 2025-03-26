import { FaTrash, FaEdit } from "react-icons/fa";

export default function ImageActions({ onDelete, onEdit }) {
  return (
    <div className="absolute inset-0 bg-black bg-opacity-0 hover:bg-opacity-50 transition-all flex items-center justify-center opacity-0 hover:opacity-100">
      <button
        onClick={(e) => {
          e.stopPropagation();
          onDelete();
        }}
        className="p-2 bg-red-600 text-white rounded-full mx-2"
      >
        <FaTrash />
      </button>
      <button
        onClick={(e) => {
          e.stopPropagation();
          onEdit();
        }}
        className="p-2 bg-blue-600 text-white rounded-full mx-2"
      >
        <FaEdit />
      </button>
    </div>
  );
}
