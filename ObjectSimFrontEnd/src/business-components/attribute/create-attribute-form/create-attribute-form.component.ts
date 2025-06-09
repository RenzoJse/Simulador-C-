import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf, NgFor, NgClass } from '@angular/common';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

import CreateAttributeModel from '../../../backend/services/class/models/create-attribute.model';
import { ClassService } from '../../../backend/services/class/class.service';

@Component({
  selector: 'app-create-attribute-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormInputComponent,
    FormButtonComponent,
    NgIf,
    NgFor,
    FormComponent,
    NgClass
  ],
  templateUrl: './create-attribute-form.component.html'
})
export class CreateAttributeFormComponent {
  @Input() tittle: string = '';
  @Output() atSubmit = new EventEmitter<CreateAttributeModel>();

  createAttributeForm: FormGroup;
  classes: { id: string; name: string }[] = [];

  createAttributeStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  Visibilities: { value: string; tag: string }[] = [
    { value: 'Public', tag: 'Public' },
    { value: 'Private', tag: 'Private' },
    { value: 'Protected', tag: 'Protected' }
  ];

  constructor(
    private formBuilder: FormBuilder,
    private classService: ClassService
  ) {
    this.createAttributeForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(10)]],
      dataTypeId: ['', Validators.required],
      visibility: ['', Validators.required],
      classId: ['', Validators.required]
    });

    this.loadClasses();
  }

  private loadClasses() {
    this.classService.getAllClasses().subscribe({
      next: (data) => {
        this.classes = data;
      },
      error: () => {
        this.classes = [];
      }
    });
  }

  public onSubmit() {
    if (this.createAttributeForm.invalid) {
      this.markAsTouched();
      return;
    }

    const formValue = this.createAttributeForm.value;

    const newAttribute: CreateAttributeModel = {
      name: formValue.name,
      dataTypeId: formValue.dataTypeId,
      visibility: formValue.visibility,
      classId: formValue.classId
    };

    this.atSubmit.emit(newAttribute);
  }

  private markAsTouched() {
    Object.values(this.createAttributeForm.controls).forEach(control => {
      control.markAsTouched();
    });
  }
}
