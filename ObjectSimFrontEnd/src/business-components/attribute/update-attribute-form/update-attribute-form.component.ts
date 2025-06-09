import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

import CreateAttributeModel from '../../../backend/services/attribute/models/create-attribute.model';

@Component({
  selector: 'app-update-attribute-form',
  standalone: true,
  imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent,FormsModule],
  templateUrl: './update-attribute-form.component.html',
})
export class UpdateAttributeFormComponent {
  @Input() title: string = '';
  @Output() atSubmit = new EventEmitter<{ id: string; model: CreateAttributeModel }>();

  updateAttributeForm: FormGroup;
  attributeId: string = '';

  visibilities = [
    { value: 'Public', tag: 'Public' },
    { value: 'Private', tag: 'Private' },
    { value: 'Protected', tag: 'Protected' }
  ];

  constructor(private fb: FormBuilder) {
    this.updateAttributeForm = this.fb.group({
      name: ['', [Validators.required]],
      dataTypeID: ['', Validators.required],
      visibility: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.updateAttributeForm.invalid || !this.attributeId) {
      this.markTouched();
      return;
    }

    const model: CreateAttributeModel = this.updateAttributeForm.value;
    this.atSubmit.emit({ id: this.attributeId, model });
  }

  private markTouched(): void {
    Object.values(this.updateAttributeForm.controls).forEach(ctrl => ctrl.markAsTouched());
  }
}
