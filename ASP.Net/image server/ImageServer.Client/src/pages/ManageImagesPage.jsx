import { useQuery } from "react-query";
import { getImages } from "../services/api";

export default function ManageImagesPage() {
  const { data: images, isLoading } = useQuery("images", getImages);

  if (isLoading) {
    return <div className="text-center py-8">Loading...</div>;
  }

  return (
    <div className="max-w-6xl mx-auto">
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Manage Images</h2>
      <div className="bg-white shadow rounded-lg">
        {/* Management interface will go here */}
        <p className="p-6 text-gray-500">Management interface coming soon...</p>
      </div>
    </div>
  );
}
