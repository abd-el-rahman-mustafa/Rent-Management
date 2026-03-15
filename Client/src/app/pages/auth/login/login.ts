import { AuthService } from './../auth.service';
import { CommonModule } from '@angular/common';
import { Component, inject, } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormInput } from '../../../shared/components/input/input';
import { ToastrService } from 'ngx-toastr';
import { passwordValidator } from '../../../shared/validators/validators';
import { LoginDto, RegisterDto } from '../auth.interface';


@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, FormInput],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  form: FormGroup = new FormGroup({});

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
    this.form = this.fb.nonNullable.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, passwordValidator]],
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

  onSubmit() {
    if (this.form.valid) {
      console.log('Form Submitted successfully:', this.form.value);
      const loginDto: LoginDto = {
        ...this.form.value
      };

      this.authService.login(loginDto).subscribe({
        next: (response) => {
          console.log('Login successful:', response);
        },
        error: (error) => {
          // this.toastr.error(error.error.message, 'Error');
        }
      });
    } else {
      this.form.markAllAsTouched();
    }
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

}
