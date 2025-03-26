import { useNavigate, useLocation, useParams } from "react-router-dom";
import { useQuery } from "react-query";
import { getImageDetails } from "../services/api";
import ImageDetails from "../components/ImageDetails";

export default function ImageDetailsPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams();
  const imageFromState = location.state?.image;

  const { data: imageFromApi, isLoading } = useQuery(
    ["imageDetails", id],
    () => getImageDetails(id),
    {
      enabled: !imageFromState,
    }
  );

  const image = imageFromState || imageFromApi;

  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-[400px]">
        <div>Loading...</div>
      </div>
    );
  }

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
