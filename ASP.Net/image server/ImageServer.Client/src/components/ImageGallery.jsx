import { useQuery, useMutation, useQueryClient } from "react-query";
import { getImages, deleteImage } from "../services/api";
import { toast } from "react-hot-toast";
import { FaTrash, FaEdit } from "react-icons/fa";
import { useNavigate } from "react-router-dom";

export default function ImageGallery() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const { data: images, isLoading } = useQuery("images", getImages);

  const deleteMutation = useMutation(deleteImage, {
    onSuccess: () => {
      queryClient.invalidateQueries("images");
      toast.success("Image deleted successfully");
    },
    onError: () => toast.error("Failed to delete image"),
  });

  if (isLoading) {
    return <div className="text-center py-8">Loading...</div>;
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mt-8">
      {images?.map((image) => (
        <div
          key={image.id}
          className="bg-white rounded-lg shadow-md overflow-hidden cursor-pointer"
          onClick={() => navigate(`/images/${image.id}`, { state: { image } })}
        >
          <div className="relative aspect-square">
            <img
              src={image.thumbnailUrl}
              alt={image.name}
              className="w-full h-full object-cover"
            />
            <div className="absolute inset-0 bg-black bg-opacity-0 hover:bg-opacity-50 transition-all flex items-center justify-center opacity-0 hover:opacity-100">
              <button
                onClick={(e) => {
                  e.stopPropagation();
                  deleteMutation.mutate(image.id);
                }}
                className="p-2 bg-red-600 text-white rounded-full mx-2"
              >
                <FaTrash />
              </button>
              <button
                onClick={(e) => {
                  e.stopPropagation();
                  navigate(`/images/${image.id}`, { state: { image } });
                }}
                className="p-2 bg-blue-600 text-white rounded-full mx-2"
              >
                <FaEdit />
              </button>
            </div>
          </div>
          <div className="p-4">
            <h3 className="font-semibold text-gray-900">{image.name}</h3>
            <p className="text-sm text-gray-500">
              {(image.fileSize / 1024).toFixed(2)} KB
            </p>
          </div>
        </div>
      ))}
    </div>
  );
}
