import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MenuComponent } from '../../components/menu/menu';
import { NavbarComponent } from '../../components/navbar/navbar';
import { BaseComponent } from '../../core/components/base-component/base-component';

@Component({
  selector: 'app-layout',
   imports: [RouterOutlet, MenuComponent, NavbarComponent],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout extends BaseComponent {
 menuCollapsed = false;

}
