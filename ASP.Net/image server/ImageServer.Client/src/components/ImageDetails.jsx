import { useQuery, useMutation, useQueryClient } from "react-query";
import { useParams, useNavigate } from "react-router-dom";
import { getImage, updateImage } from "../services/api";
import { useState } from "react";
import { toast } from "react-hot-toast";
import { FaArrowLeft, FaSave } from "react-icons/fa";

export default function ImageDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [name, setName] = useState("");
  const [file, setFile] = useState(null);
  const queryClient = useQueryClient();

  const { data: image, isLoading } = useQuery(
    ["image", id],
    () => getImage(id),
    {
      onSuccess: (data) => setName(data.name),
    }
  );

  const updateMutation = useMutation(
    ({ id, formData }) => updateImage(id, formData),
    {
      onSuccess: () => {
        queryClient.invalidateQueries(["image", id]);
        toast.success("Image updated successfully");
      },
      onError: () => toast.error("Failed to update image"),
    }
  );

  const handleSubmit = (e) => {
    e.preventDefault();
    const formData = new FormData();
    if (name !== image.name) formData.append("name", name);
    if (file) formData.append("file", file);
    updateMutation.mutate({ id, formData });
  };

  if (isLoading) return <div>Loading...</div>;
  if (!image) return <div>Image not found</div>;

  return (
    <div className="max-w-3xl mx-auto p-6">
      <button
        onClick={() => navigate(-1)}
        className="mb-6 inline-flex items-center text-blue-600 hover:text-blue-800"
      >
        <FaArrowLeft className="mr-2" /> Back
      </button>

      <div className="bg-white rounded-lg shadow-md overflow-hidden">
        <div className="p-6">
          <div className="aspect-square mb-6">
            <img
              src={image.imageUrl}
              alt={image.name}
              className="w-full h-full object-contain"
            />
          </div>

          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">
                Image Name
              </label>
              <input
                type="text"
                value={name}
                onChange={(e) => setName(e.target.value)}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">
                Replace Image
              </label>
              <input
                type="file"
                onChange={(e) => setFile(e.target.files?.[0])}
                accept="image/*"
                className="mt-1 block w-full text-sm text-gray-500
                  file:mr-4 file:py-2 file:px-4
                  file:rounded-full file:border-0
                  file:text-sm file:font-semibold
                  file:bg-blue-50 file:text-blue-700
                  hover:file:bg-blue-100"
              />
            </div>

            <div className="pt-4">
              <button
                type="submit"
                disabled={updateMutation.isLoading}
                className="w-full flex items-center justify-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50"
              >
                <FaSave className="mr-2" /> Save Changes
              </button>
            </div>
          </form>

          <div className="mt-6 border-t pt-6">
            <h3 className="text-lg font-medium text-gray-900">Image Details</h3>
            <dl className="mt-4 grid grid-cols-1 gap-4 sm:grid-cols-2">
              <div>
                <dt className="text-sm font-medium text-gray-500">Size</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {(image.fileSize / 1024).toFixed(2)} KB
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Type</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {image.contentType}
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Created</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {new Date(image.createdAt).toLocaleString()}
                </dd>
              </div>
              {image.updatedAt && (
                <div>
                  <dt className="text-sm font-medium text-gray-500">Updated</dt>
                  <dd className="mt-1 text-sm text-gray-900">
                    {new Date(image.updatedAt).toLocaleString()}
                  </dd>
                </div>
              )}
            </dl>
          </div>
        </div>
      </div>
    </div>
  );
}
