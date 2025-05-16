import { Component } from '@angular/core';
import { HeaderComponent } from './header/header.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
    standalone: true,
  template: `
    <h1>Welcome to {{title}}!</h1>

    <router-outlet><router-outlet />
  `,
    imports: [HeaderComponent, RouterOutlet],
  styles: []
})
export class AppComponent {
  title = 'ObjectSimFrontEnd';
}
