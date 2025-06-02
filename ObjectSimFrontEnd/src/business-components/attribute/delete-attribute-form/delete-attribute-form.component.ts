import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

@Component({
  selector: 'app-delete-attribute-form',
  standalone: true,
  imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent],
  templateUrl: './delete-attribute-form.component.html'
})
export class DeleteAttributeFormComponent {
  @Output() onDelete = new EventEmitter<string>();

  deleteForm: FormGroup;
  deleteStatus: { loading?: true; error?: string } | null = null;

  constructor(private fb: FormBuilder) {
    this.deleteForm = this.fb.group({
      id: ['', [Validators.required]]
    });
  }

  submitDelete(): void {
    if (this.deleteForm.invalid) {
      this.deleteForm.markAllAsTouched();
      return;
    }

    const id = this.deleteForm.value.id;
    this.onDelete.emit(id);
  }
}
