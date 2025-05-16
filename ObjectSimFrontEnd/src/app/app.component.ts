import { Component } from '@angular/core';
import { HeaderComponent } from './header/header.component';
import { RouterOutlet } from '@angular/router';
import { FooterComponent } from './footer/footer.component';

@Component({
  selector: 'app-root',
    standalone: true,
  template: `
  <app-header></app-header>
  <main class="container my-4">
    <router-outlet></router-outlet>
  </main>
  <app-footer></app-footer>
  `,
    imports: [HeaderComponent, RouterOutlet,FooterComponent],
  styles: []
})
export class AppComponent {
  title = 'ObjectSimFrontEnd';
}
