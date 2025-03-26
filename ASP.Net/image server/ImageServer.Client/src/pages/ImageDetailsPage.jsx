import { useNavigate, useLocation } from "react-router-dom";
import ImageDetails from "../components/ImageDetails";

export default function ImageDetailsPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const image = location.state?.image;

  if (!image) {
    return (
      <div className="flex justify-center items-center min-h-[400px]">
        <div className="text-red-600">Image not found</div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4">
      <ImageDetails image={image} onBack={() => navigate(-1)} />
    </div>
  );
}
