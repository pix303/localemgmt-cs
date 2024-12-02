import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserStore } from '@stores/user';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {
  userStore = inject(UserStore);

  ngOnInit() {
  }

  login = () => {
    this.userStore.login();
  }
}
