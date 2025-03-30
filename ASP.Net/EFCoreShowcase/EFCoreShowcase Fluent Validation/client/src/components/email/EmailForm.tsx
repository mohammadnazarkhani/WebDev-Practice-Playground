import React, { useState, useEffect } from 'react';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { emailApi } from '../../api';
import { Email, ValidationRulesResponse } from '../../api/types';

interface Props {
  validationRules?: ValidationRulesResponse;
}

const createValidationSchema = (rules: ValidationRulesResponse) => {
  const emailRegex = new RegExp(
    rules.properties.address.validators.find(v => v.name === 'RegularExpressionValidator')?.expression || 
    '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$'
  );

  return Yup.object().shape({
    address: Yup.string()
      .required('Email address is required')
      .matches(emailRegex, 'Invalid email format'),
    subject: Yup.string()
      .required('Subject is required')
      .max(
        rules.properties.subject.validators.find(v => v.name === 'MaximumLengthValidator')?.max || 100,
        'Subject is too long'
      ),
    message: Yup.string()
      .required('Message is required')
      .max(
        rules.properties.message.validators.find(v => v.name === 'MaximumLengthValidator')?.max || 1000,
        'Message is too long'
      )
  });
};

const EmailForm: React.FC<Props> = () => {
  const [validationSchema, setValidationSchema] = useState<any>(null);
  const [submitError, setSubmitError] = useState('');
  const [success, setSuccess] = useState('');

  useEffect(() => {
    const loadValidationRules = async () => {
      try {
        const rules = await emailApi.getValidationRules();
        setValidationSchema(createValidationSchema(rules));
      } catch (err) {
        console.error('Failed to load validation rules:', err);
        setSubmitError('Failed to load validation rules');
      }
    };

    loadValidationRules();
  }, []);

  const formik = useFormik({
    initialValues: {
      address: '',
      subject: '',
      message: ''
    },
    validationSchema,
    enableReinitialize: true,
    onSubmit: async (values: Email) => {
      try {
        await emailApi.sendEmail(values);
        setSuccess('Email sent successfully!');
        formik.resetForm();
      } catch (err) {
        setSubmitError('Failed to send email');
      }
    }
  });

  if (!validationSchema) {
    return <div>Loading validation rules...</div>;
  }

  return (
    <form onSubmit={formik.handleSubmit} className="space-y-4">
      <div>
        <label htmlFor="address" className="block text-sm font-medium text-gray-700">
          Email Address
        </label>
        <input
          id="address"
          type="email"
          {...formik.getFieldProps('address')}
          className={`mt-1 block w-full rounded-md border ${
            formik.touched.address && formik.errors.address ? 'border-red-500' : 'border-gray-300'
          } px-3 py-2`}
        />
        {formik.touched.address && formik.errors.address && (
          <p className="mt-1 text-sm text-red-500">{formik.errors.address}</p>
        )}
      </div>

      <div>
        <label htmlFor="subject" className="block text-sm font-medium text-gray-700">
          Subject
        </label>
        <input
          id="subject"
          type="text"
          {...formik.getFieldProps('subject')}
          className={`mt-1 block w-full rounded-md border ${
            formik.touched.subject && formik.errors.subject ? 'border-red-500' : 'border-gray-300'
          } px-3 py-2`}
        />
        {formik.touched.subject && formik.errors.subject && (
          <p className="mt-1 text-sm text-red-500">{formik.errors.subject}</p>
        )}
      </div>

      <div>
        <label htmlFor="message" className="block text-sm font-medium text-gray-700">
          Message
        </label>
        <textarea
          id="message"
          {...formik.getFieldProps('message')}
          rows={4}
          className={`mt-1 block w-full rounded-md border ${
            formik.touched.message && formik.errors.message ? 'border-red-500' : 'border-gray-300'
          } px-3 py-2`}
        />
        {formik.touched.message && formik.errors.message && (
          <p className="mt-1 text-sm text-red-500">{formik.errors.message}</p>
        )}
      </div>

      {submitError && <p className="text-red-500">{submitError}</p>}
      {success && <p className="text-green-500">{success}</p>}

      <button
        type="submit"
        disabled={formik.isSubmitting}
        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:bg-blue-300"
      >
        {formik.isSubmitting ? 'Sending...' : 'Send Email'}
      </button>
    </form>
  );
};

export default EmailForm;
