export default function ImageMetadata({ image }) {
  return (
    <div className="p-6 space-y-6 border-t md:border-t-0 md:border-l border-gray-200">
      <div>
        <h2 className="text-2xl font-bold text-gray-900 mb-2">{image.name}</h2>
        <div className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-blue-50 text-blue-700">
          {(image.fileSize / 1024).toFixed(2)} KB
        </div>
      </div>

      <div className="space-y-4">
        <h3 className="text-lg font-medium text-gray-900 pb-2 border-b">
          Image Details
        </h3>
        <dl className="grid grid-cols-1 gap-y-4">
          <div>
            <dt className="text-sm font-medium text-gray-500">Type</dt>
            <dd className="mt-1 text-sm text-gray-900">{image.contentType}</dd>
          </div>
          <div>
            <dt className="text-sm font-medium text-gray-500">Created</dt>
            <dd className="mt-1 text-sm text-gray-900">
              {new Date(image.createdAt).toLocaleString()}
            </dd>
          </div>
          {image.updatedAt && (
            <div>
              <dt className="text-sm font-medium text-gray-500">
                Last Modified
              </dt>
              <dd className="mt-1 text-sm text-gray-900">
                {new Date(image.updatedAt).toLocaleString()}
              </dd>
            </div>
          )}
        </dl>
      </div>
    </div>
  );
}
