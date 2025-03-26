import { Link, useLocation } from "react-router-dom";
import classNames from "classnames";

export default function Header() {
  const location = useLocation();

  const navItems = [
    { path: "/", label: "Gallery" },
    { path: "/upload", label: "Upload" },
    { path: "/manage", label: "Manage" },
  ];

  return (
    <header className="bg-white shadow">
      <div className="container mx-auto px-4 py-6">
        <div className="flex items-center justify-between">
          <h1 className="text-3xl font-bold text-gray-900">Image Gallery</h1>
          <nav className="flex space-x-4">
            {navItems.map(({ path, label }) => (
              <Link
                key={path}
                to={path}
                className={classNames(
                  "px-3 py-2 rounded-md text-sm font-medium",
                  location.pathname === path
                    ? "bg-gray-900 text-white"
                    : "text-gray-700 hover:bg-gray-100"
                )}
              >
                {label}
              </Link>
            ))}
          </nav>
        </div>
      </div>
    </header>
  );
}
