import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { Input } from '../../../shared/components/input/input';
import { Countdown } from '../../../shared/pipelines/countdown-pipe';

@Component({
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule, Input,Countdown],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register implements OnDestroy {
  registerForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    otp: new FormControl(''),
    password: new FormControl(''),
    confirmPassword: new FormControl(''),
  });

  otpSent = false;
  otpCountdown = 180; // 3 minutes
  intervalId: any;

  sendOtp() {
    if (
      this.registerForm.get('firstName')?.valid &&
      this.registerForm.get('lastName')?.valid &&
      this.registerForm.get('email')?.valid
    ) {
      this.otpSent = true;

      // Add required validators for OTP and password fields once OTP is sent
      this.registerForm.get('otp')?.setValidators([Validators.required]);
      this.registerForm.get('otp')?.updateValueAndValidity();
      this.registerForm.get('password')?.setValidators([Validators.required]);
      this.registerForm.get('password')?.updateValueAndValidity();
      this.registerForm.get('confirmPassword')?.setValidators([Validators.required]);
      this.registerForm.get('confirmPassword')?.updateValueAndValidity();

      this.startCountdown();
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
      console.log('Form Submitted successfully:', this.registerForm.value);
      // Here you would typically call your authentication service
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
