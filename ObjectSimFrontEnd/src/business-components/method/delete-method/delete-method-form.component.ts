import { Component, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';
import { MethodDropdownComponent } from '../dropdown/method-dropdown.component';

@Component({
  selector: 'app-delete-method-form',
  standalone: true,
  imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent,
    NgIf, FormComponent, MethodDropdownComponent],
  templateUrl: './delete-method-form.component.html'
})

export class DeleteMethodFormComponent {
  @Input() title: string = '';
  @Output() atDelete = new EventEmitter<string | undefined>();

  deleteMethodForm: FormGroup;

  deleteMethodFormStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  MethodId: string | undefined;

  constructor(private fb: FormBuilder, private cdr: ChangeDetectorRef) {
    this.deleteMethodForm = this.fb.group({
      methodId: ['']
    });
  }

  public onSubmit() {
    if (this.deleteMethodForm.valid) {
      const {methodId} = this.deleteMethodForm.value;
      this.atDelete.emit(this.deleteMethodForm.value.methodId);
    } else {
      this.markAsTouched();
      console.log('Invalid form:', this.deleteMethodForm.errors);
    }
  }

  private markAsTouched() {
    Object.values(this.deleteMethodForm.controls).forEach(control => {
      control.markAsTouched();
    });
  }

  updateSelectedMethodId(event: { methodId: string | undefined; }) {
    this.MethodId = event.methodId;
    this.deleteMethodForm.patchValue({ methodId: event.methodId });
    this.cdr.detectChanges();
  }

}