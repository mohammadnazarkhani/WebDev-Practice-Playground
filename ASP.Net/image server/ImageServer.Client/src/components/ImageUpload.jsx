import { useState } from "react";
import { useMutation, useQueryClient } from "react-query";
import { uploadImage } from "../services/api";
import { toast } from "react-hot-toast";
import { FaUpload } from "react-icons/fa";

export default function ImageUpload() {
  const [file, setFile] = useState(null);
  const [name, setName] = useState("");
  const queryClient = useQueryClient();

  const mutation = useMutation(uploadImage, {
    onSuccess: () => {
      queryClient.invalidateQueries("images");
      toast.success("Image uploaded successfully");
      setFile(null);
      setName("");
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
    <form onSubmit={handleSubmit} className="bg-white shadow rounded-lg p-6">
      <div className="space-y-4">
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

        <button
          type="submit"
          disabled={mutation.isLoading}
          className="w-full flex items-center justify-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50"
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
  );
}
