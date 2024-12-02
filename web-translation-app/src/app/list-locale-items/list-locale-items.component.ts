import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, } from '@angular/forms';
import { SearchIconComponent } from '../icons/search/search.component';
import { environment } from '../../environments/environment';
import { LocaleStore } from '@stores/locale';

@Component({
  selector: 'app-list-locale-items',
  standalone: true,
  imports: [SearchIconComponent, ReactiveFormsModule],
  templateUrl: './list-locale-items.component.html',
  styleUrl: './list-locale-items.component.css'
})
export class ListLocaleItemsComponent {
  store = inject(LocaleStore);
  localeitems = this.store.items;
  filters = new FormGroup({
    lang: new FormControl(""),
    content: new FormControl(""),
  });

  test = environment.auth0Domain;

  constructor() {
    this.filters.valueChanges
      .subscribe(
        v => this.store.setFilters(v.lang, v.content)
      );
  }
}

