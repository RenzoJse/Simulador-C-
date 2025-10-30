import { Component, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

import { ClassDropdownComponent } from '../../class/dropdown/class-dropdown.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

@Component({
  selector: 'app-update-class-name-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgIf,
    ClassDropdownComponent,
    FormButtonComponent,
    FormComponent
  ],
  templateUrl: './update-class-form.component.html'
})
export class UpdateClassFormComponent {
  @Input() loading? = false;
  @Input() error?: string | null = null;
  @Output() atSubmit = new EventEmitter<{ classId: string; newName: string }>();

  form: FormGroup;
  classId: string | undefined;

  formStatus: {
    loading?: true;
    error?: string;
  } = {};

  constructor(private fb: FormBuilder, private cdr: ChangeDetectorRef) {
    this.form = this.fb.group({
      classId: [''],
      newName: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  onSubmit(): void {
    if (this.form.valid && this.classId) {
      const newName = this.form.value.newName;
      this.atSubmit.emit({ classId: this.classId, newName });
    } else {
      this.markAsTouched();
    }
  }

onSubmitWithData(data: any): void {
  if (this.form.valid && this.classId) {
    this.atSubmit.emit({
      classId: this.classId,
      newName: data.newName
    });
  } else {
    this.markAsTouched();
  }
}

  private markAsTouched() {
    Object.values(this.form.controls).forEach(control => control.markAsTouched());
  }

  updateClassId(event: { classId: string | undefined }): void {
    this.classId = event.classId;
    this.form.patchValue({ classId: event.classId });
    this.cdr.detectChanges();
  }
}