export const passwordValidator = (control: any) => {
  const value = control.value;
  if (!value) {
    return null;
  }
  const hasUpperCase = /[A-Z]/.test(value);
  const hasLowerCase = /[a-z]/.test(value);
  const hasNumbers = /\d/.test(value);
  const hasSpecialChar = /[!@#$%^&*()-+]/.test(value);
  const lengthValid = value.length >= 8;
  const isValid = hasUpperCase && hasLowerCase && hasNumbers && hasSpecialChar && lengthValid;
  return isValid ? null : { password: true };
};


export const onlyNumbersValidator = (control: any) => {
  const value = control.value;
  if (!value) {
    return null;
  }
  const isValid = /^\d+$/.test(value);
  return isValid ? null : { onlyNumbers: true };
}

export const otpValidator = (control: any) => {
  const value = control.value;
  if (!value) {
    return null;
  }
  const isValid = /^\d{6}$/.test(value);
  return isValid ? null : { otp: true };
}