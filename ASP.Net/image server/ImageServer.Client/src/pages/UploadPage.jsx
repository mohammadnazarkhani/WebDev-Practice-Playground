import ImageUpload from "../components/ImageUpload";

export default function UploadPage() {
  return (
    <div className="max-w-2xl mx-auto">
      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Upload New Image
      </h2>
      <ImageUpload />
    </div>
  );
}
