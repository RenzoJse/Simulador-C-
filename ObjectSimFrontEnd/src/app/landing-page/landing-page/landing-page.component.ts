import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent {
  images = [
    {
      path: 'assets/images/image1.jpg',
      author: 'Joaquín Calvo'
    },
    {
      path: 'assets/images/image4.png',
      author: 'Renzo José'
    },
    {
      path: 'assets/images/image5.png',
      author: 'Matias Fontes'
    }
  ];
}
