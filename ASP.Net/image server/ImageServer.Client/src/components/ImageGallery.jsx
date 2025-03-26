import { useQuery, useMutation, useQueryClient } from "react-query";
import { getImages, deleteImage } from "../services/api";
import { toast } from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import ImageCard from "./ImageCard/ImageCard";

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

  const handleNavigate = (type, imageData) => {
    if (type === "edit") {
      navigate(`/images/${imageData.id}/edit`, { state: { image: imageData } });
    } else {
      navigate(`/images/${imageData.id}`, { state: { image: imageData } });
    }
  };

  if (isLoading) {
    return <div className="text-center py-8">Loading...</div>;
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {images?.map((image) => (
        <ImageCard
          key={image.id}
          image={image}
          onDelete={(id) => deleteMutation.mutate(id)}
          onNavigate={(path) => handleNavigate(path, image)}
        />
      ))}
    </div>
  );
}
