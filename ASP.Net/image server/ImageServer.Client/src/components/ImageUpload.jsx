import { useState } from "react";
import { useMutation, useQueryClient } from "react-query";
import { uploadImage } from "../services/api";
import { toast } from "react-hot-toast";
import { FaUpload, FaImage } from "react-icons/fa";

export default function ImageUpload() {
  const [file, setFile] = useState(null);
  const [preview, setPreview] = useState(null);
  const [name, setName] = useState("");
  const queryClient = useQueryClient();

  const handleFileChange = (e) => {
    const selectedFile = e.target.files?.[0];
    if (selectedFile) {
      setFile(selectedFile);
      setPreview(URL.createObjectURL(selectedFile));
      if (!name) {
        setName(selectedFile.name.split(".")[0]);
      }
    }
  };

  const mutation = useMutation(uploadImage, {
    onSuccess: () => {
      queryClient.invalidateQueries("images");
      toast.success("Image uploaded successfully");
      setFile(null);
      setName("");
      setPreview(null);
    },
    onError: () => toast.error("Failed to upload image"),
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!file || !name) {
      toast.error("Please provide both a name and a file");
      return;
    }

    const formData = new FormData();
    formData.append("file", file);
    formData.append("name", name);
    mutation.mutate(formData);
  };

  return (
    <div className="grid grid-cols-1 md:grid-cols-2">
      {/* Preview Section */}
      <div className="p-6 flex items-center justify-center bg-gray-50 border-b md:border-b-0 md:border-r border-gray-200">
        <div className="w-full aspect-square rounded-lg overflow-hidden bg-gray-100 flex items-center justify-center">
          {preview ? (
            <img
              src={preview}
              alt="Preview"
              className="w-full h-full object-contain"
            />
          ) : (
            <div className="text-center p-8">
              <FaImage className="mx-auto h-12 w-12 text-gray-400" />
              <p className="mt-2 text-sm text-gray-500">
                Image preview will appear here
              </p>
            </div>
          )}
        </div>
      </div>

      {/* Upload Form */}
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
              placeholder="Enter image name"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">
              Image File
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
                    {file ? file.name : "Click to select a file"}
                  </p>
                </div>
              </label>
            </div>
          </div>

          <button
            type="submit"
            disabled={mutation.isLoading}
            className="w-full flex items-center justify-center px-4 py-3 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 transition-colors"
          >
            {mutation.isLoading ? (
              "Uploading..."
            ) : (
              <>
                <FaUpload className="mr-2" /> Upload Image
              </>
            )}
          </button>
        </div>
      </form>
    </div>
  );
}
