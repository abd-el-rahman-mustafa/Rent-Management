import { Component, input, OnDestroy, OnInit, signal, forwardRef, Self, computed, Optional } from '@angular/core';
import { InputType } from '../../interfaces/input.interface';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NgControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-input',
  imports: [],
  templateUrl: './input.html',
  styleUrl: './input.css',

})
export class Input implements ControlValueAccessor {
  // use signals to track input state
  label = input<string>('');
  placeholder = input<string>('');
  type = input<string>('');
  error = input<string>('');

  value: string = '';
  disabled: boolean = false;

  constructor(@Self() @Optional() public ngControl: NgControl) {
    if (this.ngControl) {
      this.ngControl.valueAccessor = this;
    }
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
}
