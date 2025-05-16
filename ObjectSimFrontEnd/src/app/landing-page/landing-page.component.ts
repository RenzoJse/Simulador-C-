import { Component } from '@angular/core';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [HeaderComponent],
  template: `
<app-header></app-header>
  `,
  styles: ``
})
export class LandingPageComponent {

}
