import ImageActions from "./ImageActions";

export default function ImageCard({ image, onDelete, onNavigate }) {
  return (
    <div
      className="bg-white rounded-lg shadow-md overflow-hidden cursor-pointer"
      onClick={() => onNavigate(image)}
    >
      <div className="relative aspect-square">
        <img
          src={image.thumbnailUrl}
          alt={image.name}
          className="w-full h-full object-cover"
        />
        <ImageActions
          onDelete={() => onDelete(image.id)}
          onEdit={() => onNavigate(image)}
        />
      </div>
      <div className="p-4">
        <h3 className="font-semibold text-gray-900">{image.name}</h3>
        <p className="text-sm text-gray-500">
          {(image.fileSize / 1024).toFixed(2)} KB
        </p>
      </div>
    </div>
  );
}
