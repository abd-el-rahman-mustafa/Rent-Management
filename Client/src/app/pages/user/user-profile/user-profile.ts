import { Component, inject, OnInit, Input } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user-profile',
  imports: [CommonModule, ReactiveFormsModule, DatePipe],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.css',
})
export class UserProfile implements OnInit {

  @Input() userId!: string | number;

  private fb = inject(FormBuilder);
  private userService = inject(UserService);

  mode: 'preview' | 'edit' = 'preview';
  user: any = null;
  isLoading = false;
  isSaving = false;
  saveSuccess = false;

  profileForm: FormGroup = this.fb.group({
    firstName:   ['', Validators.required],
    lastName:    ['', Validators.required],
    email:       ['', [Validators.required, Validators.email]],
    phone:       [''],
    gender:      [''],
    dateOfBirth: [''],
    active:      [true],
  });

  ngOnInit() {
    this.loadUser();
  }

  loadUser() {
    this.isLoading = true;
    this.userService.getUserById(this.userId).subscribe({
      next: (res: any) => {
        this.user = res;
        this.profileForm.patchValue({
          firstName:   res.firstName   ?? '',
          lastName:    res.lastName    ?? '',
          email:       res.email       ?? '',
          phone:       res.phone       ?? '',
          gender:      res.gender      ?? '',
          dateOfBirth: res.dateOfBirth ? res.dateOfBirth.split('T')[0] : '',
          active:      res.active      ?? true,
        });
        this.isLoading = false;
      },
      error: () => { this.isLoading = false; }
    });
  }

  setMode(mode: 'preview' | 'edit') {
    this.mode = mode;
    this.saveSuccess = false;
  }

  toggleActive() {
    const current = this.profileForm.get('active')?.value;
    this.profileForm.get('active')?.setValue(!current);
  }

  onSubmit() {
    if (this.profileForm.invalid) return;
    this.isSaving = true;
    this.saveSuccess = false;

    this.userService.updateUser(this.userId, this.profileForm.value).subscribe({
      next: (res: any) => {
        this.user = { ...this.user, ...this.profileForm.value };
        this.isSaving = false;
        this.saveSuccess = true;
        setTimeout(() => {
          this.saveSuccess = false;
          this.setMode('preview');
        }, 1500);
      },
      error: () => { this.isSaving = false; }
    });
  }
}