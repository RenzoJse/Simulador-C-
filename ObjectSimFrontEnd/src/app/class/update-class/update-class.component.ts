import { Component, Inject } from '@angular/core';
import { ClassApiRepository } from '../../../backend/repositories/class-api-repository.service';
import { UpdateClassFormComponent } from '../../../business-components/class/update-class/update-class-form.component';

@Component({
  selector: 'app-update-class-name',
  standalone: true,
  imports: [UpdateClassFormComponent],
  templateUrl: './update-class.component.html'
})
export class UpdateClassComponent {
  status: { loading?: true; error?: string } = {};

  constructor(
    @Inject(ClassApiRepository) private readonly _classRepo: ClassApiRepository
  ) {}

  atSubmit(data: { classId: string; newName: string }) {
    this.status = { loading: true };

    this._classRepo.updateClass(data.classId, { name: data.newName }).subscribe({
      next: () => {
        this.status = {};
        alert('Nombre actualizado con éxito');
      },
      error: (error: any) => {
        this.status = {
          error: error?.message || 'Error al actualizar el nombre.'
        };
      }
    });
  }
}