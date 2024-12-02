import { Routes } from '@angular/router';
import { ListLocaleItemsComponent } from './list-locale-items/list-locale-items.component';
import { UserProfileComponent } from './user-profile/user-profile.component';

export const routes: Routes = [
  { path: "list", component: ListLocaleItemsComponent },
  { path: "user-profile", component: UserProfileComponent },
];

