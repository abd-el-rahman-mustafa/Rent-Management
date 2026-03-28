import { UserService } from './../user.service';
import { AuthService } from './../../auth/auth.service';
import { Component, inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-users-lit',
  imports: [],
  templateUrl: './users-lit.html',
  styleUrl: './users-lit.css',
})
export class UsersLit implements OnInit {

  UserService = inject(UserService);

  ngOnInit() {
    this.getAllUsers();
  }


  getAllUsers() {
    this.UserService.getAllUsers().subscribe((res) => {
      console.log(res);
    });

  }





}
