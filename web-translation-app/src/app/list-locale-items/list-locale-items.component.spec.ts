import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListLocaleItemsComponent } from './list-locale-items.component';

describe('ListLocaleItemsComponent', () => {
  let component: ListLocaleItemsComponent;
  let fixture: ComponentFixture<ListLocaleItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListLocaleItemsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListLocaleItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
