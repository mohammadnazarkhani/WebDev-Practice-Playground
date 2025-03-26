import { FaArrowLeft } from "react-icons/fa";
import ImagePreview from "./ImagePreview";
import ImageMetadata from "./ImageMetadata";

export default function ImageDetails({ image, onBack }) {
  if (!image) {
    return (
      <div className="text-center py-8">
        <div className="text-red-600">Image not found</div>
      </div>
    );
  }

  return (
    <div className="max-w-6xl mx-auto p-6">
      <button
        onClick={onBack}
        className="mb-6 inline-flex items-center text-blue-600 hover:text-blue-800"
      >
        <FaArrowLeft className="mr-2" /> Back to Gallery
      </button>

      <div className="bg-white rounded-lg shadow-md overflow-hidden">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
          <ImagePreview imageUrl={image.imageUrl} name={image.name} />
          <ImageMetadata image={image} />
        </div>
      </div>
    </div>
  );
}
