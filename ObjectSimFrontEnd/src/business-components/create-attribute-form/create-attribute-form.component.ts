import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { NgIf } from '@angular/common';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormComponent } from '../../components/form/form/form.component';

@Component({
  selector: 'app-create-attribute-form',
  template: './create-attribute-form.component.html',
  standalone: true,
  imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent],
  styles: ['./create-attribute-form.component.css']
})
export class CreateAttributeFormComponent {
createAttributeForm: FormGroup;
  createAttributeStatus: { loading: boolean; error?: string } | null = null;

  constructor(private fb: FormBuilder, private http: HttpClient) {
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

    this.createAttributeStatus = { loading: true };

    const formData = this.createAttributeForm.value;

    this.http.post('/api/attributes', formData).subscribe({
      next: () => {
        this.createAttributeStatus = null;
        this.createAttributeForm.reset();
        alert('Attribute created successfully!');
      },
      error: (err) => {
        this.createAttributeStatus = {
          loading: false,
          error: 'Failed to create attribute.'
        };
        console.error(err);
      }
    });
  }

  get dataTypeKinds(): string[] {
    return ['Value', 'Reference'];
  }

  get visibilities(): string[] {
    return ['Public', 'Private', 'Protected'];
  }
}
