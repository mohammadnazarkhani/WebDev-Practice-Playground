import ImageUpload from "../components/ImageUpload";
import { useNavigate } from "react-router-dom";
import { FaArrowLeft } from "react-icons/fa";

export default function UploadPage() {
  const navigate = useNavigate();

  return (
    <div className="max-w-4xl mx-auto">
      <div className="relative mb-8">
        <button
          onClick={() => navigate(-1)}
          className="absolute left-0 top-1/2 -translate-y-1/2 inline-flex items-center text-gray-600 hover:text-gray-900"
        >
          <FaArrowLeft className="mr-2" /> Back
        </button>
        <div className="text-center">
          <h2 className="text-3xl font-bold text-gray-900">Upload New Image</h2>
          <p className="mt-2 text-gray-600">
            Share your images with the world. Supported formats: JPG, PNG, GIF
          </p>
        </div>
      </div>
      <div className="bg-white rounded-xl shadow-lg overflow-hidden">
        <ImageUpload />
      </div>
    </div>
  );
}
