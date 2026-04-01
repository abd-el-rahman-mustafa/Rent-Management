import { TokenService } from './../../../core/services/token.service';
import { AuthService } from './../auth.service';
import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormInput } from '../../../shared/components/input/input';
import { ToastrService } from 'ngx-toastr';
import { otpValidator, passwordValidator } from '../../../shared/validators/validators';
import { LoginDto, LoginOtpDto } from '../auth.interface';
import { Countdown } from '../../../shared/pipelines/countdown-pipe';
import { Router, RouterLink } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-component/base-component';
import { LangRouterLinkDirective } from '../../../core/directives/lang-router-link.directive';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, FormInput, Countdown, RouterLink, LangRouterLinkDirective],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login extends BaseComponent {
  requestForm: FormGroup = new FormGroup({});
  otpLoginForm: FormGroup = new FormGroup({});

  otpSent = false;
  otpCountdown = 60;
  intervalId: any;

  // Used in the brand panel footer
  readonly currentYear = signal(new Date().getFullYear());

  toastr = inject(ToastrService);
  router = inject(Router);
  tokenService = inject(TokenService);

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
  ) {
    super();
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
      otp: ['', [Validators.required, otpValidator]],
    });
  }

  startCountdown() {
    this.otpCountdown = 60;
    if (this.intervalId) clearInterval(this.intervalId);
    this.intervalId = setInterval(() => {
      this.otpCountdown--;
      if (this.otpCountdown <= 0) clearInterval(this.intervalId);
    }, 1000);
  }

  sendLoginRequest() {
    if (this.requestForm.valid) {
      const loginDto: LoginDto = { ...this.requestForm.value };
      this.authService.loginRequest(loginDto).subscribe({
        next: (response) => {
          this.toastr.success(response.detail, response.title);
          this.otpSent = true;
          this.startCountdown();
        },
      });
    } else {
      this.requestForm.markAllAsTouched();
    }
  }

  verifyOtp() {
    if (this.otpLoginForm.valid) {
      const otpDto: LoginOtpDto = {
        email: this.requestForm.value.email,
        otp: this.otpLoginForm.value.otp,
      };
      this.authService.emailOtpLogin(otpDto).subscribe({
        next: (response) => {
          this.toastr.success(response.detail, response.title);
          this.tokenService.setToken(response.data);
          this.router.navigate(['/']);
        },
      });
    } else {
      this.otpLoginForm.markAllAsTouched();
    }
  }

  reSendOtp() {
    const loginDto: LoginDto = { ...this.requestForm.value };
    this.authService.loginRequest(loginDto).subscribe({
      next: (response) => {
        this.toastr.success(response.detail, response.title);
        this.startCountdown();
      },
    });
  }

  ngOnDestroy() {
    if (this.intervalId) clearInterval(this.intervalId);
  }
}