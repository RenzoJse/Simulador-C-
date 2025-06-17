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
    { id: 'guide1',
      title: 'Simulation Guide',
      description: 'Step-by-step instructions to run your simulations efficiently.',
      link: ''
    },
    { id: 'guide2',
      title: 'Output Model Guide',
      description: 'You can customize how method execution results are displayed by uploading your own response transformer (.DLL).\n' +
          'Go to the "Output Models" section, upload your custom plugin, and choose it from the menu during simulation. ' +
          'This allows dynamic formatting (e.g., HTML, XML) without recompiling the application.',
      link: ''
    },
    {
      id: 'guide3',
      title: 'More Information',
      description: 'If you need more information or assistance, please refer to the official documentation on our GitHub repository. ' +
          'There you will find detailed guides, examples, and answers to frequently asked questions.',
      link: 'https://github.com/IngSoft-DA2/240272_203832_248142'
    }
  ];

  guideFooter = 'All guides are subject to change and continuous improvement.';
}
