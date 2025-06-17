import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfoGuideComponent } from "../../../components/info-guides/info-guide.component";

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [CommonModule, InfoGuideComponent],
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent {

  images = [
    'assets/images/image1.jpg',
    'assets/images/image4.png',
    'assets/images/image5.png'
  ];

  guideItems = [
    { id: 'guide1', title: 'Simulation Guide', description: 'Step-by-step instructions to run your simulations efficiently.' },
    { id: 'guide2', title: 'Output Model Guide', description: 'Learn how to interpret and manage the generated output models.' },
    { id: 'guide3', title: 'Additional Resources', description: 'Explore other helpful guides and documentation.' }
  ];

  guideFooter = 'All guides are subject to change and continuous improvement.';
}
