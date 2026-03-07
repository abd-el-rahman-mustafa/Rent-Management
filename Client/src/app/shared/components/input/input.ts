import { Component, input, OnDestroy, OnInit, signal, forwardRef } from '@angular/core';
import { InputType } from '../../interfaces/input.interface';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-input',
  imports: [],
  templateUrl: './input.html',
  styleUrl: './input.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => Input),
      multi: true,
    },
  ],
})
export class Input implements OnInit, OnDestroy, ControlValueAccessor {
  // use signals to track input state
  label = input<string>('');
  placeholder = input<string>('');
  type = input<string>('');
  error = input<string>('');

  value: string = '';
  disabled: boolean = false;

  ngOnInit(): void { }

  onChange: (value: string) => void = () => { };
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

  ngOnDestroy(): void { }
}
