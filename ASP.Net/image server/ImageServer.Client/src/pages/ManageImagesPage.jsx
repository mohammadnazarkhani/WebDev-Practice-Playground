import { useQuery, useMutation, useQueryClient } from "react-query";
import { useNavigate } from "react-router-dom";
import { getImages, deleteImage, deleteAllImages } from "../services/api";
import { toast } from "react-hot-toast";
import { FaEdit, FaTrash, FaEye, FaSync } from "react-icons/fa";

export default function ManageImagesPage() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const { data: images, isLoading, refetch } = useQuery("images", getImages);

  const deleteMutation = useMutation(deleteImage, {
    onSuccess: () => {
      queryClient.invalidateQueries("images");
      toast.success("Image deleted successfully");
    },
    onError: () => toast.error("Failed to delete image"),
  });

  const deleteAllMutation = useMutation(deleteAllImages, {
    onSuccess: () => {
      queryClient.invalidateQueries("images");
      toast.success("All images deleted successfully");
    },
    onError: () => toast.error("Failed to delete all images"),
  });

  const handleRefresh = () => {
    refetch();
    toast.success("Image list refreshed");
  };

  const handleDeleteAll = () => {
    if (
      window.confirm(
        "Are you sure you want to delete ALL images? This action cannot be undone!"
      )
    ) {
      deleteAllMutation.mutate();
    }
  };

  if (isLoading) {
    return <div className="text-center py-8">Loading...</div>;
  }

  return (
    <div className="max-w-5xl mx-auto">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold text-gray-900">Manage Images</h2>
        <div className="flex gap-2">
          <button
            onClick={handleRefresh}
            className="inline-flex items-center px-4 py-2 bg-gray-100 hover:bg-gray-200 text-gray-800 rounded-lg"
          >
            <FaSync className="mr-2" /> Refresh
          </button>
          <button
            onClick={handleDeleteAll}
            className="inline-flex items-center px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded-lg"
            disabled={!images?.length}
          >
            <FaTrash className="mr-2" /> Delete All
          </button>
        </div>
      </div>

      <div className="bg-white shadow-md rounded-lg overflow-hidden">
        <ul className="divide-y divide-gray-200">
          {images?.map((image) => (
            <li
              key={image.id}
              className="p-4 hover:bg-gray-50 transition-colors"
            >
              <div className="flex items-center justify-between">
                <div className="flex items-center space-x-4">
                  <div className="w-16 h-16 flex-shrink-0">
                    <img
                      src={image.thumbnailUrl}
                      alt={image.name}
                      className="w-full h-full object-cover rounded-md"
                    />
                  </div>
                  <div>
                    <h3 className="text-lg font-medium text-gray-900">
                      {image.name}
                    </h3>
                    <p className="text-sm text-gray-500">
                      {(image.fileSize / 1024).toFixed(2)} KB ·{" "}
                      {new Date(image.createdAt).toLocaleDateString()}
                    </p>
                  </div>
                </div>

                <div className="flex items-center space-x-2">
                  <button
                    onClick={() => navigate(`/images/${image.id}`)}
                    className="p-2 text-blue-600 hover:bg-blue-50 rounded-full"
                    title="View Details"
                  >
                    <FaEye />
                  </button>
                  <button
                    onClick={() =>
                      navigate(`/images/${image.id}/edit`, {
                        state: { image },
                      })
                    }
                    className="p-2 text-green-600 hover:bg-green-50 rounded-full"
                    title="Edit Image"
                  >
                    <FaEdit />
                  </button>
                  <button
                    onClick={() => {
                      if (
                        window.confirm(
                          "Are you sure you want to delete this image?"
                        )
                      ) {
                        deleteMutation.mutate(image.id);
                      }
                    }}
                    className="p-2 text-red-600 hover:bg-red-50 rounded-full"
                    title="Delete Image"
                  >
                    <FaTrash />
                  </button>
                </div>
              </div>
            </li>
          ))}
        </ul>
        {images?.length === 0 && (
          <div className="text-center py-8 text-gray-500">No images found</div>
        )}
      </div>
    </div>
  );
}
