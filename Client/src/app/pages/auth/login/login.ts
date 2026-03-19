import { AuthService } from './../auth.service';
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormInput } from '../../../shared/components/input/input';
import { ToastrService } from 'ngx-toastr';
import { otpValidator, passwordValidator } from '../../../shared/validators/validators';
import { LoginDto, LoginOtpDto, LoginResponse, RegisterDto } from '../auth.interface';
import { Countdown } from '../../../shared/pipelines/countdown-pipe';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, FormInput, Countdown],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  requestForm: FormGroup = new FormGroup({});
  otpLoginForm: FormGroup = new FormGroup({});

  otpSent = false;
  otpCountdown = 60; // 60 seconds
  intervalId: any;

  toastr = inject(ToastrService);
  router = inject(Router);
  /**
   *
   */
  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }
  initializeForm() {
    this.requestForm = this.fb.nonNullable.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, passwordValidator]],
    });
    this.otpLoginForm = this.fb.nonNullable.group({
      otp: ['', [Validators.required, otpValidator]],
    });
  }

  startCountdown() {
    this.otpCountdown = 60; // reset to 60 seconds
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

  sendLoginRequest() {
    if (this.requestForm.valid) {
      console.log('Form Submitted successfully:', this.requestForm.value);
      const loginDto: LoginDto = {
        ...this.requestForm.value,
      };

      this.authService.loginRequest(loginDto).subscribe({
        next: (response) => {
          this.toastr.success(response.details, response.title);

          this.otpSent = true;
          this.startCountdown();
        },
        error: (error) => {
          // this.toastr.error(error.error.message, 'Error');
        },
      });
    } else {
      this.requestForm.markAllAsTouched();
    }
  }

  verifyOtp() {
    if (this.otpLoginForm.valid) {
      console.log('OTP Form Submitted successfully:', this.otpLoginForm.value);
      const otpDto: LoginOtpDto = {
        email: this.requestForm.value.email,
        otp: this.otpLoginForm.value.otp,
      };

      this.authService.emailOtpLogin(otpDto).subscribe({
        next: (response) => {
          //  store token and user data in local storage or a service

          this.toastr.success(response.details, response.title);
          this.saveToken(response.data);

          // navigate to home page or dashboard
          this.router.navigate(['/']);
        },
        error: (error) => {
          // this.toastr.error(error.error.message, 'Error');
        },
      });
    } else {
      this.otpLoginForm.markAllAsTouched();
    }
  }

  reSendOtp() {
    const loginDto: LoginDto = {
      ...this.requestForm.value,
    };
    this.authService.loginRequest(loginDto).subscribe({
      next: (response) => {
        this.toastr.success(response.details, response.title);
        this.startCountdown();
      },
      error: (error) => {
        // this.toastr.error(error.error.message, 'Error');
      },
    });
  }
  saveToken(token: LoginResponse) {
    localStorage.setItem('accessToken', token.accessToken);
    localStorage.setItem('accessTokenExpires', token.accessTokenExpires);
  }
  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
}
