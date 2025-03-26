import { useState } from "react";
import { useNavigate, useLocation, useParams } from "react-router-dom";
import { useMutation, useQueryClient } from "react-query";
import { updateImage, patchImage } from "../services/api";
import { toast } from "react-hot-toast";
import { FaUpload, FaImage, FaArrowLeft } from "react-icons/fa";

export default function EditImagePage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams();
  const image = location.state?.image;
  const queryClient = useQueryClient();

  const [name, setName] = useState(image?.name || "");
  const [file, setFile] = useState(null);
  const [preview, setPreview] = useState(image?.imageUrl);

  const handleFileChange = (e) => {
    const selectedFile = e.target.files?.[0];
    if (selectedFile) {
      setFile(selectedFile);
      setPreview(URL.createObjectURL(selectedFile));
    }
  };

  const updateMutation = useMutation(
    ({ id, formData }) => {
      const isNameChanged = name !== image.name;
      const isFileChanged = file !== null;

      // If both fields are changed, use PUT
      if (isNameChanged && isFileChanged) {
        return updateImage(id, formData);
      }
      // If only one field is changed, use PATCH
      return patchImage(id, formData);
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(["images"]);
        queryClient.invalidateQueries(["imageDetails", id]);
        toast.success("Image updated successfully");
        navigate(-1);
      },
      onError: () => toast.error("Failed to update image"),
    }
  );

  const handleSubmit = (e) => {
    e.preventDefault();

    const formData = new FormData();
    if (name !== image.name) formData.append("name", name);
    if (file) formData.append("file", file);

    // Don't submit if nothing has changed
    if (name === image.name && !file) {
      toast.error("No changes detected");
      return;
    }

    updateMutation.mutate({ id, formData });
  };

  if (!image) {
    return <div className="text-center py-8 text-red-600">Image not found</div>;
  }

  return (
    <div className="max-w-4xl mx-auto">
      <div className="relative mb-8">
        <button
          onClick={() => navigate(-1)}
          className="absolute left-0 top-1/2 -translate-y-1/2 inline-flex items-center text-gray-600 hover:text-gray-900"
        >
          <FaArrowLeft className="mr-2" /> Back
        </button>
        <div className="text-center">
          <h2 className="text-3xl font-bold text-gray-900">Edit Image</h2>
          <p className="mt-2 text-gray-600">Update image details</p>
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-lg overflow-hidden">
        <div className="grid grid-cols-1 md:grid-cols-2">
          {/* Preview Section */}
          <div className="p-6 flex items-center justify-center bg-gray-50 border-b md:border-b-0 md:border-r border-gray-200">
            <div className="w-full aspect-square rounded-lg overflow-hidden bg-gray-100">
              {preview ? (
                <img
                  src={preview}
                  alt="Preview"
                  className="w-full h-full object-contain"
                />
              ) : (
                <div className="flex items-center justify-center h-full">
                  <FaImage className="h-12 w-12 text-gray-400" />
                </div>
              )}
            </div>
          </div>

          {/* Edit Form */}
          <form onSubmit={handleSubmit} className="p-6">
            <div className="space-y-6">
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
                <div className="mt-1">
                  <input
                    type="file"
                    onChange={handleFileChange}
                    accept="image/*"
                    className="hidden"
                    id="file-upload"
                  />
                  <label
                    htmlFor="file-upload"
                    className="cursor-pointer w-full flex items-center justify-center px-6 py-3 border-2 border-dashed border-gray-300 rounded-lg hover:border-blue-500 transition-colors"
                  >
                    <div className="text-center">
                      <FaUpload className="mx-auto h-6 w-6 text-gray-400" />
                      <p className="mt-2 text-sm text-gray-500">
                        {file ? file.name : "Click to select a new image"}
                      </p>
                    </div>
                  </label>
                </div>
              </div>

              <div className="flex gap-4">
                <button
                  type="button"
                  onClick={() => navigate(-1)}
                  className="flex-1 py-2 px-4 border border-gray-300 rounded-md text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={updateMutation.isLoading}
                  className="flex-1 py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50"
                >
                  {updateMutation.isLoading ? "Saving..." : "Save Changes"}
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
