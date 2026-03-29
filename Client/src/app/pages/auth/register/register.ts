import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { FormInput } from '../../../shared/components/input/input';
import { Countdown } from '../../../shared/pipelines/countdown-pipe';
import { AuthService } from '../auth.service';
import { RegisterDto } from '../auth.interface';
import { otpValidator, passwordValidator } from '../../../shared/validators/validators';
import { ToastrService } from 'ngx-toastr';
import { LangRouterLinkDirective } from '../../../core/directives/lang-router-link.directive';

@Component({
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule, FormInput, Countdown,LangRouterLinkDirective],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register implements OnInit, OnDestroy {
  registerForm: FormGroup = new FormGroup({});

  otpSent = false;
  otpCountdown = 180; // 3 minutes
  intervalId: any;

  toastr = inject(ToastrService);
  /**
   *
   */
  constructor(private authService: AuthService, private fb: FormBuilder,) {

  }

  ngOnInit(): void {
    this.initializeForm();
  }
  initializeForm() {
    this.registerForm = this.fb.nonNullable.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      emailOtpCode: [''],
      password: ['', []],
      confirmPassword: ['']
    });
  }
  sendOtp() {
    if (
      this.registerForm.get('firstName')?.valid &&
      this.registerForm.get('lastName')?.valid &&
      this.registerForm.get('email')?.valid
    ) {
      this.otpSent = true;

      // Add required validators for OTP and password fields once OTP is sent
      this.registerForm.get('emailOtpCode')?.setValidators([Validators.required, otpValidator]);
      this.registerForm.get('emailOtpCode')?.updateValueAndValidity();
      this.registerForm.get('password')?.setValidators([Validators.required, passwordValidator]);
      this.registerForm.get('password')?.updateValueAndValidity();
      this.registerForm.get('confirmPassword')?.setValidators([Validators.required, passwordValidator]);
      this.registerForm.get('confirmPassword')?.updateValueAndValidity();

      this.startCountdown();

      this.authService.sendOtp(this.registerForm.get('email')?.value).subscribe({
        next: (res: any) => {
          this.toastr.success(res.message, 'Success');
        },
        error: (error) => {
          // this.toastr.error('An error occurred while sending OTP. Please try again.', 'Error');
        }
      });
    } else {
      // Mark fields as touched to show validation errors for initial fields
      this.registerForm.get('firstName')?.markAsTouched();
      this.registerForm.get('lastName')?.markAsTouched();
      this.registerForm.get('email')?.markAsTouched();
    }
  }

  startCountdown() {
    this.otpCountdown = 180;
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
    this.intervalId = setInterval(() => {
      this.otpCountdown--;
      if (this.otpCountdown <= 0) {
        clearInterval(this.intervalId);
      }
    }, 1000);
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const registerDto: RegisterDto = {
        ...this.registerForm.value
      };

      this.authService.register(registerDto).subscribe({
        next: (response) => {
        },
        error: (error) => {
          // this.toastr.error(error.error.message, 'Error');
        }
      });
    } else {
      this.registerForm.markAllAsTouched();
    }
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
}
