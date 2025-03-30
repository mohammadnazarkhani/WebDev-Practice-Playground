export interface ValidationErrorDetail {
  property: string;
  error: string;
}

export class ValidationError extends Error {
  errors: ValidationErrorDetail[];

  constructor(message: string, errors: ValidationErrorDetail[]) {
    super(message);
    this.name = 'ValidationError';
    this.errors = errors;
  }
}
