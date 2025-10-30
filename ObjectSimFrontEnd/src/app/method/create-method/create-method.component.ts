import { Component, Inject } from '@angular/core';
import { MethodService } from '../../../backend/services/method/method.service';
import MethodCreateModel from '../../../backend/services/method/models/method-dto.model';
import MethodDTO from '../../../backend/services/method/models/method-dto.model';

@Component({
  selector: 'app-create-method',
  templateUrl: './create-method.component.html',
  styleUrls: ['./create-method.component.css']
})
export class CreateMethodComponent {
  status: { loading?: true; error?: string } | null = null;
  createdMethod: MethodDTO | null = null;

  savedClassId = '';
  savedModifiers: string[] = [];

  constructor(
    @Inject(MethodService) private readonly _methodService: MethodService
  ) {}

  protected atSubmit(method: MethodCreateModel) {
    this.status = { loading: true };
    this.savedClassId = method.classId;
    this.savedModifiers = [];
    if (method.isAbstract)  this.savedModifiers.push('Abstract');
    if (method.isSealed)    this.savedModifiers.push('Sealed');
    if (method.isOverride)  this.savedModifiers.push('Override');
    if (method.isVirtual)   this.savedModifiers.push('Virtual');
    if (method.isStatic)    this.savedModifiers.push('Static');

    this._methodService.createMethod(method).subscribe({
      next: (response) => {
        this.status = null;
        this.createdMethod = response;
      },
      error: (err: any) => {
        this.status = { error: err.error?.message || err.message || 'Error creating method.' };
      }
    });
  }
}