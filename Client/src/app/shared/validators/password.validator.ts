export const passwordValidator = (control: any) => {
  const value = control.value;
  if (!value) {
    return null;
  }
  const hasUpperCase = /[A-Z]/.test(value);
  const hasLowerCase = /[a-z]/.test(value);
  const hasNumbers = /\d/.test(value);
  const hasSpecialChar = /[!@#$%^&*()-+]/.test(value);
  const lengthValid = value.length >= 8 ;
  const isValid = hasUpperCase && hasLowerCase && hasNumbers && hasSpecialChar && lengthValid;
  return isValid ? null : { password: true };
};