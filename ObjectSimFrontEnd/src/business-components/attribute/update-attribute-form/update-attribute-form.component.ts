import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

import AttributeUpdateModel from '../../../backend/services/attribute/models/attribute-update.model';

@Component({
  selector: 'app-update-attribute-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    FormInputComponent,
    FormButtonComponent,
    FormComponent
  ],
  templateUrl: './update-attribute-form.component.html',
  styleUrl: './update-attribute-form.component.css'
})
export class UpdateAttributeFormComponent implements OnChanges {
  @Input() title: string = '';
  @Input() attribute: AttributeUpdateModel | null = null;

  @Output() atSubmit = new EventEmitter<AttributeUpdateModel>();

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
      visibility: ['', Validators.required],
      classId: ['', Validators.required],
      isStatic: [false, Validators.required],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['attribute'] && this.attribute) {
      this.updateAttributeForm.patchValue(this.attribute);
    }
  }

  onSubmit(): void {
    if (this.updateAttributeForm.invalid || !this.attribute?.id) {
      this.updateAttributeForm.markAllAsTouched();
      return;
    }

    const updatedModel: AttributeUpdateModel = {
      id: this.attribute.id,
      ...this.updateAttributeForm.value
    };

    this.atSubmit.emit(updatedModel);
  }
}
