import { QueryClient, QueryClientProvider } from "react-query";
import { Toaster } from "react-hot-toast";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Header from "./components/Header";
import UploadPage from "./pages/UploadPage";
import ImageDetailsPage from "./pages/ImageDetailsPage";
import GalleryPage from "./pages/GalleryPage";
import EditImagePage from "./pages/EditImagePage";
import ManageImagesPage from "./pages/ManageImagesPage";

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <div className="min-h-screen bg-gray-100">
          <Header />
          <main className="container mx-auto px-4 py-8">
            <Routes>
              <Route path="/" element={<GalleryPage />} />
              <Route path="/upload" element={<UploadPage />} />
              <Route path="/images/:id" element={<ImageDetailsPage />} />
              <Route path="/images/:id/edit" element={<EditImagePage />} />
              <Route path="/manage" element={<ManageImagesPage />} />
            </Routes>
          </main>
          <Toaster position="bottom-right" />
        </div>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;
