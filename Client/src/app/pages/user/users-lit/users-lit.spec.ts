import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersLit } from './users-lit';

describe('UsersLit', () => {
  let component: UsersLit;
  let fixture: ComponentFixture<UsersLit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsersLit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UsersLit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
