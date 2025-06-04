import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import CreateNamespaceModel from '../../../backend/services/namespace/model/create-namespace.model';
import { FormComponent } from '../../../components/form/form/form.component';
import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';

@Component({
  selector: 'app-create-namespace-form',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, FormComponent, FormInputComponent, FormButtonComponent],
  templateUrl: './create-namespace-form.component.html'
})
export class CreateNamespaceFormComponent {
  @Output() nsSubmit = new EventEmitter<CreateNamespaceModel>();

  form: FormGroup;
  createStatus: { loading?: boolean; error?: string } | null = null;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(10)]],
      parentId: ['']
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.markAsTouched();
      return;
    }

    const value: CreateNamespaceModel = this.form.value;
    this.nsSubmit.emit(value);
  }

  private markAsTouched() {
    Object.values(this.form.controls).forEach(c => c.markAsTouched());
  }
}
