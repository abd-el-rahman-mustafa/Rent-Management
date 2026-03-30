import { Component, input, OnDestroy, OnInit, signal, forwardRef, Self, computed, Optional, inject } from '@angular/core';
import { InputType } from '../../interfaces/input.interface';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR, NgControl, Validators } from '@angular/forms';
import { KeyValuePipe } from '@angular/common';
import { LanguageService } from '../../../core/services/language.service';

@Component({
  selector: 'app-input',
  imports: [KeyValuePipe],
  templateUrl: './input.html',
  styleUrl: './input.css',

})
export class FormInput implements ControlValueAccessor {
  // use signals to track input state
  label = input<string>('');
  placeholder = input<string>('');
  type = input<'text' | 'number' | 'email' | 'password'>('text');
  errorMsg: string = '';
  value: string = '';
  disabled: boolean = false;

  // inject language service to get current language
  lang = inject(LanguageService).lang();

  placeholderDir = computed(() => {
    const text = this.placeholder();
    if (!text) return 'ltr';
    // check if first meaningful character is arabic
    const firstChar = text.trim()[0];
    return /[\u0600-\u06FF]/.test(firstChar) ? 'rtl' : 'ltr';
  });

  constructor(@Self() @Optional() public ngControl: NgControl) {
    if (this.ngControl) {
      this.ngControl.valueAccessor = this;
    }
  }
  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
  onChange: (value: string) => void = () => {

  };
  onTouched: () => void = () => { };

  handleInput(event: Event): void {
    const val = (event.target as HTMLInputElement).value;
    this.value = val;
    this.onChange(val);


  }

  writeValue(value: string): void {
    this.value = value || '';
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
  get isRequired(): boolean {
    return this.ngControl?.control?.hasValidator?.(Validators.required) ?? false;
  }

  getErrorMessage(key: string, value: any): string {

    const enErrorMessages: { [key: string]: string } = {
      required: 'This field is required',
      email: 'Invalid email address',
      password: 'Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character and be greater or equal to 8 characters long',
      onlyNumbers: 'Only numbers are allowed',
      otp: 'OTP must be a 6-digit number'
    };
    const arErrorMessages: { [key: string]: string } = {
      required: 'هذا الحقل مطلوب',
      email: 'عنوان البريد الإلكتروني غير صالح',
      password: 'يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل، حرف صغير واحد، رقم واحد، وحرف خاص واحد وأن تكون 8 أحرف أو أكثر',
      onlyNumbers: 'يسمح بالأرقام فقط',
      otp: 'يجب أن يكون OTP رقمًا مكونًا من 6 أرقام'
    };
    return (this.lang === 'en' ? enErrorMessages[key] : arErrorMessages[key]) || 'Invalid field';

  }
}
