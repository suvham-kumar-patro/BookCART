import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserProfile } from '../../core/models/user-profile.model';
import { UserService } from '../../core/services/user.service';

@Component({
  selector: 'app-user-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss'
})
export class UserProfileComponent implements OnInit {
  profile?: UserProfile;
  editMode:boolean = false;
  phoneNumber:string = '';

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {
    this.userService.getProfile().subscribe((data) => {
      this.profile = data;
      this.phoneNumber = data.phoneNumber;
    });
  }

  saveProfile() {
    this.userService.updateProfile({ phoneNumber: this.phoneNumber }).subscribe(() => {
      this.editMode = false;
      this.loadProfile();
    });
  }
}
