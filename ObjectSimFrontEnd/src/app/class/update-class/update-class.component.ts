import { Component, Inject } from '@angular/core';
import { ClassApiRepository } from '../../../backend/repositories/class-api-repository.service';
import { UpdateClassFormComponent } from '../../../business-components/class/update-class/update-class-form.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-class-name',
  standalone: true,
  imports: [UpdateClassFormComponent, CommonModule],
  templateUrl: './update-class.component.html',
  styleUrl: './update-class.component.css'
})
export class UpdateClassComponent {
  status: { loading?: true; error?: string } = {};
  updatedClassName: string | null = null;

  constructor(
    @Inject(ClassApiRepository) private readonly _classRepo: ClassApiRepository
  ) {}

  atSubmit(data: { classId: string; newName: string }) {
    this.status = { loading: true };

    this._classRepo.updateClass(data.classId, { name: data.newName }).subscribe({
      next: () => {
        this.status = {};
        this.updatedClassName = data.newName;
      },
      error: (error: any) => {
        this.status = {
          error: error?.message || 'Error al actualizar el nombre.'
        };
      }
    });
  }
}