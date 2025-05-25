import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MethodService } from '../../../services/method.service'; 
import { MethodDTO } from '../../../models/method-dto.model'; 

@Component({
  selector: 'app-create-method',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-method.component.html',
  styleUrls: ['./create-method.component.css']
})

export class CreateMethodComponent {
  method: MethodDTO = {
    name: '',
    type: {
      name: '',
      type: ''
    },
    accessibility: 'Private',
    isAbstract: false,
    isSealed: false,
    isOverride: false,
    isVirtual: false,
    classId: '',
    localVariables: [],
    parameters: []
  };

  constructor(private methodService: MethodService) {}

  createMethod() {
    this.methodService.createMethod(this.method).subscribe({
      next: (response) => {
        alert('Método creado: ' + response.name);
      },
      error: (error) => {
        alert('Error al crear método: ' + error.error.message);
      }
    });
  }

  protected readonly onsubmit = onsubmit;
}