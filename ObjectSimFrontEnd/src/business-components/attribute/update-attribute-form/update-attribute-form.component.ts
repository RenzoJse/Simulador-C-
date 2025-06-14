import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

import CreateAttributeModel from '../../../backend/services/attribute/models/create-attribute.model';

@Component({
  selector: 'app-update-attribute-form',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent],
  templateUrl: './update-attribute-form.component.html'
})
export class UpdateAttributeFormComponent implements OnChanges {
  @Input() title: string = '';
  @Input() attribute: CreateAttributeModel | null = null;
  @Input() attributeId: string = '';
  @Output() atSubmit = new EventEmitter<{ id: string; model: CreateAttributeModel }>();

  updateAttributeForm: FormGroup;

  visibilities = [
    { value: 'Public', tag: 'Public' },
    { value: 'Private', tag: 'Private' },
    { value: 'Protected', tag: 'Protected' }
  ];

  constructor(private fb: FormBuilder) {
    this.updateAttributeForm = this.fb.group({
      name: ['', Validators.required],
      dataTypeId: ['', Validators.required],
      visibility: ['', Validators.required]
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['attribute'] && this.attribute) {
      this.updateAttributeForm.patchValue(this.attribute);
    }
  }

  onSubmit(): void {
    if (this.updateAttributeForm.invalid || !this.attributeId) {
      this.updateAttributeForm.markAllAsTouched();
      return;
    }

    const model: CreateAttributeModel = this.updateAttributeForm.value;
    this.atSubmit.emit({ id: this.attributeId, model });
  }
}
