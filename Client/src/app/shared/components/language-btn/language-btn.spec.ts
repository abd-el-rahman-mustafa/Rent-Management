import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LanguageBtn } from './language-btn';

describe('LanguageBtn', () => {
  let component: LanguageBtn;
  let fixture: ComponentFixture<LanguageBtn>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LanguageBtn]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LanguageBtn);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
