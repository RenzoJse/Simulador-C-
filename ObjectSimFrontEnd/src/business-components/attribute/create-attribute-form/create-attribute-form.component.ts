import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule, NgIf } from '@angular/common';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';
import { ClassDropdownComponent } from '../../class/dropdown/class-dropdown.component';
import { DataTypeDropdownComponent } from '../../datatype/datatype-dropdown/data-type-dropdown.component';
import CreateAttributeModel from '../../../backend/services/attribute/models/create-attribute.model';

@Component({
  selector: 'app-create-attribute-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    NgIf,
    FormInputComponent,
    FormButtonComponent,
    FormComponent,
    ClassDropdownComponent,
    DataTypeDropdownComponent
  ],
  templateUrl: './create-attribute-form.component.html'
})
export class CreateAttributeFormComponent {
  @Input() title: string = '';
  @Output() atSubmit = new EventEmitter<CreateAttributeModel>();

  createAttributeForm: FormGroup;
  selectedClassId: string = '';
  selectedDataTypeId: string = '';

  createAttributeStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  Visibilities = [
    { value: 'Public', tag: 'Public' },
    { value: 'Private', tag: 'Private' },
    { value: 'Protected', tag: 'Protected' }
  ];

  Static = [
    { value: 'true', tag: 'Static' },
    { value: 'false', tag: 'Non-Static' }
  ];

  constructor(private formBuilder: FormBuilder) {
    this.createAttributeForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(10)]],
      visibility: ['', Validators.required],
      isStatic: ['false', Validators.required]
    });
  }

  onSubmit() {
    if (this.createAttributeForm.invalid || !this.selectedClassId || !this.selectedDataTypeId) {
      this.markAsTouched();
      return;
    }

    const formValue = this.createAttributeForm.value;

    const newAttribute: CreateAttributeModel = {
      name: formValue.name,
      dataTypeId: this.selectedDataTypeId,
      visibility: formValue.visibility,
      classId: this.selectedClassId,
      isStatic: formValue.isStatic === 'true'
    };

    this.atSubmit.emit(newAttribute);
  }

  onClassSelected(event: { classId: string | undefined }) {
    this.selectedClassId = event.classId ?? '';
  }

  onDataTypeSelected(dataTypeId: string) {
    this.selectedDataTypeId = dataTypeId;
  }

  private markAsTouched() {
    Object.values(this.createAttributeForm.controls).forEach(control => control.markAsTouched());
  }
}
