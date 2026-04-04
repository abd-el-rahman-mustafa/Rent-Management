import { Component, inject } from '@angular/core';
import { LoadingService } from '../../services/loading.service';
import { BaseComponent } from '../base-component/base-component';

@Component({
  selector: 'app-spinner',
  imports: [],
  templateUrl: './spinner.html',
  styleUrl: './spinner.css',
})
export class Spinner extends BaseComponent{
 loadingService = inject(LoadingService);
}
