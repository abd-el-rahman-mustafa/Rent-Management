import { Component, inject, OnDestroy } from '@angular/core';
import { LanguageService } from '../../services/language.service';

@Component({
  selector: 'app-base-component',
  imports: [],
  templateUrl: './base-component.html',
  styleUrl: './base-component.css',
})
export class BaseComponent {

  private langService = inject(LanguageService);

  lang = this.langService.lang;

}
