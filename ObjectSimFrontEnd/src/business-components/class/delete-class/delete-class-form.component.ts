import {Component, Input, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClassDropdownComponent } from '../../class/dropdown/class-dropdown.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

@Component({
  selector: 'app-delete-class-form',
  standalone: true,
  imports: [ CommonModule, ReactiveFormsModule, NgIf,
    ClassDropdownComponent, FormButtonComponent, FormComponent],
  templateUrl: './delete-class-form.component.html'
})
export class DeleteClassFormComponent {
  @Input() loading = false;
  @Input() error: string | null = null;
  @Output() atSubmit = new EventEmitter<string>();

  deleteClassform: FormGroup;
  classID: string | undefined;

  deleteFormStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(private fb: FormBuilder, private cdr: ChangeDetectorRef) {
    this.deleteClassform = this.fb.group({
      classId: ['']
    });
  }

  public onSubmit() {
    if (this.deleteClassform.valid) {
      this.atSubmit.emit(this.classID);
    } else {
      this.markAsTouched();
      console.log('Invalid form:', this.deleteClassform.errors);
    }
  }

  private markAsTouched() {
    Object.values(this.deleteClassform.controls).forEach(control => {
      control.markAsTouched();
    });
  }

  updateClassId(event: { classId: string | undefined; }) {
    this.classID = event.classId;
    this.deleteClassform.patchValue({ classID: event.classId });
    this.cdr.detectChanges();
  }
}