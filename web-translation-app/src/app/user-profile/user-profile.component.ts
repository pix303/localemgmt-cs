import { Component, OnInit, inject } from '@angular/core';
import { UserStore } from '@stores/user';
import { WarningComponent } from 'app/icons/warning/warning.component';

@Component({
  selector: 'app-user-profile',
  imports: [WarningComponent],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {
  userStore = inject(UserStore);
  user = this.userStore.user;
}
