import { useParams, useNavigate } from "react-router-dom";

export default function EditImagePage() {
  const { id } = useParams();
  const navigate = useNavigate();

  return (
    <div className="max-w-2xl mx-auto">
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Edit Image</h2>
      <div className="bg-white shadow rounded-lg p-6">
        {/* Edit form will go here */}
        <p className="text-gray-500">Edit form coming soon...</p>
      </div>
    </div>
  );
}
