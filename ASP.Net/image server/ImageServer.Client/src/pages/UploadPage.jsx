import ImageUpload from "../components/ImageUpload";

export default function UploadPage() {
  return (
    <div className="max-w-4xl mx-auto">
      <div className="text-center mb-8">
        <h2 className="text-3xl font-bold text-gray-900">Upload New Image</h2>
        <p className="mt-2 text-gray-600">
          Share your images with the world. Supported formats: JPG, PNG, GIF
        </p>
      </div>
      <div className="bg-white rounded-xl shadow-lg overflow-hidden">
        <ImageUpload />
      </div>
    </div>
  );
}
