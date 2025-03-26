import { QueryClient, QueryClientProvider } from 'react-query';
import { Toaster } from 'react-hot-toast';
import ImageGallery from './components/ImageGallery';
import ImageUpload from './components/ImageUpload';
import Header from './components/Header';

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <div className="min-h-screen bg-gray-100">
        <Header />
        <main className="container mx-auto px-4 py-8">
          <ImageUpload />
          <ImageGallery />
        </main>
        <Toaster position="bottom-right" />
      </div>
    </QueryClientProvider>
  );
}

export default App;
