import { Component, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormComponent } from '../../components/form/form/form.component';
import AttributeDto from '../../backend/services/attribute/model/attribute-dto.model';

@Component({
  selector: 'app-create-attribute-form',
  templateUrl: './create-attribute-form.component.html',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormInputComponent,
    FormButtonComponent,
    NgIf,
    FormComponent
  ],
  styleUrls: ['./create-attribute-form.component.css']
})
export class CreateAttributeFormComponent {
  @Output() atSubmit = new EventEmitter<AttributeDto>();

  createAttributeForm: FormGroup;
  createAttributeStatus: { loading: boolean; error?: string } | null = null;

  constructor(private fb: FormBuilder) {
    this.createAttributeForm = this.fb.group({
      name: ['', Validators.required],
      dataTypeName: ['', Validators.required],
      dataTypeKind: ['', Validators.required],
      visibility: ['', Validators.required],
      classId: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.createAttributeForm.invalid) return;

    const formData: AttributeDto = this.createAttributeForm.value;
    this.atSubmit.emit(formData); // ✅ lo emite al padre
  }

  get dataTypeKinds(): { value: string; tag: string }[] {
    return [
      { value: 'Value', tag: 'Value Type' },
      { value: 'Reference', tag: 'Reference Type' }
    ];
  }

  get visibilities(): { value: string; tag: string }[] {
    return [
      { value: 'Public', tag: 'Public' },
      { value: 'Private', tag: 'Private' },
      { value: 'Protected', tag: 'Protected' }
    ];
  }
}
