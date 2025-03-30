import PropTypes from "prop-types";

export default function Input({ label, type = "text", error, ...props }) {
  // Ensure props is not null/undefined before spreading
  const safeProps = props ?? {};

  return (
    <div className="space-y-1">
      {label && (
        <label className="block text-sm font-medium text-gray-700">
          {label}
        </label>
      )}
      <input
        type={type}
        className={`block w-full px-3 py-2 border rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm ${
          error ? "border-red-500" : "border-gray-300"
        }`}
        {...safeProps}
      />
      {error && <p className="text-sm text-red-600">{error}</p>}
    </div>
  );
}

Input.propTypes = {
  label: PropTypes.string,
  type: PropTypes.string,
  error: PropTypes.string,
};

Input.defaultProps = {
  type: "text",
};
