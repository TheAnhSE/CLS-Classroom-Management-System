import React, { forwardRef } from 'react';

interface InputFieldProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  helperText?: string;
}

export const InputField = forwardRef<HTMLInputElement, InputFieldProps>(
  ({ label, error, helperText, className = '', id, ...props }, ref) => {
    const generatedId = id || Math.random().toString(36).substring(7);

    return (
      <div className={`w-full ${className}`}>
        {label && (
          <label htmlFor={generatedId} className="block text-sm font-medium text-slate-700 mb-1.5">
            {label}
          </label>
        )}
        <input
          id={generatedId}
          ref={ref}
          className={`
            block w-full rounded-lg border px-3 py-2 text-sm transition-colors
            focus:outline-none focus:ring-2 focus:ring-offset-1
            disabled:cursor-not-allowed disabled:bg-slate-50 disabled:text-slate-500
            ${
              error
                ? 'border-red-300 text-red-900 focus:border-red-500 focus:ring-red-500 placeholder:text-red-300'
                : 'border-slate-300 text-slate-900 focus:border-blue-500 focus:ring-blue-500 placeholder:text-slate-400'
            }
          `}
          {...props}
        />
        {(error || helperText) && (
          <p className={`mt-1.5 text-sm ${error ? 'text-red-600' : 'text-slate-500'}`}>
            {error || helperText}
          </p>
        )}
      </div>
    );
  }
);

InputField.displayName = 'InputField';
