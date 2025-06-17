import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import CreateNamespaceModel from '../../../backend/services/namespace/model/create-namespace.model';

import { FormComponent } from '../../../components/form/form/form.component';
import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { NamespaceDropdownComponent } from '../namespace-dropdown/namespace-dropdown.component';
import { MultiSelectComponent } from '../multi-selector/multi-select.component';

@Component({
  selector: 'app-create-namespace-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgIf,
    FormComponent,
    FormInputComponent,
    FormButtonComponent,
    NamespaceDropdownComponent,
    MultiSelectComponent
  ],
  templateUrl: './create-namespace-form.component.html',
  styleUrl: './create-namespace-form.component.css',
})
export class CreateNamespaceFormComponent {
  @Output() nsSubmit = new EventEmitter<CreateNamespaceModel>();

  form: FormGroup;
  createStatus: { loading?: boolean; error?: string } | null = null;

  selectedClassIds: string[] = [];

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

    const raw = this.form.value;
    const value: CreateNamespaceModel = {
      name: raw.name,
      parentId: raw.parentId?.trim() !== '' ? raw.parentId : null,
      classIds: this.selectedClassIds
    };

    this.nsSubmit.emit(value);
  }

  onSelectNamespace(event: { namespaceId: string | undefined }) {
    this.form.get('parentId')?.setValue(event.namespaceId || '');
  }

  onClassSelect(ids: string[]) {
    this.selectedClassIds = ids;
  }

  private markAsTouched() {
    Object.values(this.form.controls).forEach(c => c.markAsTouched());
  }
}
