export default function ImagePreview({ imageUrl, name }) {
  return (
    <div className="p-6">
      <div className="aspect-square">
        <img
          src={imageUrl}
          alt={name}
          className="w-full h-full object-contain rounded-lg border border-gray-200"
        />
      </div>
    </div>
  );
}
