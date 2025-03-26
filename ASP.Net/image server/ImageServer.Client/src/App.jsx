import { QueryClient, QueryClientProvider } from "react-query";
import { Toaster } from "react-hot-toast";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import ImageGallery from "./components/ImageGallery";
import ImageDetails from "./components/ImageDetails";
import Header from "./components/Header";
import UploadPage from "./pages/UploadPage";

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <div className="min-h-screen bg-gray-100">
          <Header />
          <main className="container mx-auto px-4 py-8">
            <Routes>
              <Route path="/" element={<ImageGallery />} />
              <Route path="/upload" element={<UploadPage />} />
              <Route path="/images/:id" element={<ImageDetails />} />
            </Routes>
          </main>
          <Toaster position="bottom-right" />
        </div>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;
