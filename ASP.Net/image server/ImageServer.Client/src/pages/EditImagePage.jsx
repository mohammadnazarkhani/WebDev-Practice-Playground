import { useParams, useNavigate, useLocation } from "react-router-dom";

export default function EditImagePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const image = location.state?.image;

  if (!image) {
    return (
      <div className="text-center py-8">
        <div className="text-red-600">Image not found</div>
      </div>
    );
  }

  return (
    <div className="max-w-2xl mx-auto">
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Edit Image</h2>
      <div className="bg-white shadow rounded-lg p-6">
        {/* Edit form will go here */}
        <p className="text-gray-500">Editing image: {image.name}</p>
      </div>
    </div>
  );
}
