import React, { useState, useEffect } from 'react';
import { emailApi } from '../../api';
import { ValidationRulesResponse } from '../../api/types';
import EmailForm from '../../components/email/EmailForm';

export default function EmailPage() {
  const [validationRules, setValidationRules] = useState<ValidationRulesResponse | undefined>();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const loadValidationRules = async () => {
      try {
        const rules = await emailApi.getValidationRules();
        setValidationRules(rules);
      } catch (err) {
        setError('Failed to load validation rules');
      } finally {
        setLoading(false);
      }
    };

    loadValidationRules();
  }, []);

  if (loading) return <div>Loading validation rules...</div>;
  if (error) return <div className="text-red-500">{error}</div>;

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Send Email</h1>
      <EmailForm validationRules={validationRules} />
    </div>
  );
}
