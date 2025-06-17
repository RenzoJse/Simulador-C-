import { Component } from '@angular/core';

@Component({
  selector: 'app-landing-page',
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

  guideItems = [
    {
      id: 'guide1',
      title: 'Simulation Guide',
      description:
        '1. Selecciona la clase de referencia y la instancia.\n' +
        '   El simulador requiere que elijas una clase de referencia (por ejemplo, "Vehiculo") y una instancia concreta (por ejemplo, "Auto"). La instancia debe ser válida respecto a la clase de referencia.\n' +
        '\n' +
        '2. Selecciona el método a ejecutar.\n' +
        '   Elige el método que deseas simular (por ejemplo, "IniciarViaje"). El método debe estar presente en la instancia o en la clase de referencia.\n' +
        '\n' +
        '3. Ejecuta la simulación.\n' +
        '   El sistema validará que la instancia y el método sean correctos. Si todo es válido, se ejecutará el método y se generará un resultado de simulación.\n' +
        '\n' +
        '4. Visualiza el resultado.\n' +
        '   El resultado de la ejecución se transforma usando el modelo de salida seleccionado, permitiendo ver la información de la simulación de manera clara y personalizada.\n',
      link: ''
    },
    {
      id: 'guide2',
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
