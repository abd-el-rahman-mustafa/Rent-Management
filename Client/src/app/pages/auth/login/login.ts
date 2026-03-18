import { AuthService } from './../auth.service';
import { CommonModule } from '@angular/common';
import { Component, inject, } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormInput } from '../../../shared/components/input/input';
import { ToastrService } from 'ngx-toastr';
import { otpValidator, passwordValidator } from '../../../shared/validators/validators';
import { LoginDto, LoginOtpDto, RegisterDto } from '../auth.interface';
import { Countdown } from '../../../shared/pipelines/countdown-pipe';


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
  otpCountdown = 30; // 30 seconds
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
    this.requestForm = this.fb.nonNullable.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, passwordValidator]],
    });
    this.otpLoginForm = this.fb.nonNullable.group({
      email: ['', [Validators.required, Validators.email]],
      otp: ['', [Validators.required, otpValidator]],
    });
  }
  sendOtp() {

  }

  startCountdown() {
    this.otpCountdown = 30;
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
        ...this.requestForm.value
      };

      this.authService.loginRequest(loginDto).subscribe({
        next: (response) => {
          this.toastr.success(response.details, response.title);

          // set email in otp form for convenience
          this.otpLoginForm.patchValue({ email: loginDto.email });
          this.otpSent = true;
          this.startCountdown();

        },
        error: (error) => {
          // this.toastr.error(error.error.message, 'Error');
        }
      });
    } else {
      this.requestForm.markAllAsTouched();
    }
  }

  verifyOtp() {
    if (this.otpLoginForm.valid) {
      console.log('OTP Form Submitted successfully:', this.otpLoginForm.value);
      const otpDto: LoginOtpDto = {
        ...this.otpLoginForm.value
      };

      this.authService.emailOtpLogin(otpDto).subscribe({
        next: (response) => {
        //  store token and user data in local storage or a service
          
          console.log('OTP Login successful:', response);
          // localStorage.setItem('token', response.token);
          // localStorage.setItem('user', JSON.stringify(response.user));
        },
        error: (error) => {
          // this.toastr.error(error.error.message, 'Error');
        }
      });
    }
    else {
      this.otpLoginForm.markAllAsTouched();
    }
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

}
