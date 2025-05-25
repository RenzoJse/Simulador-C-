import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormComponent } from '../../components/form/form/form.component';

import MethodCreateModel from '../../backend/services/method/models/method-dto.model';

@Component({
  selector: 'app-create-method',
  standalone: true,
  imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent],
  templateUrl: './create-method-form.component.html',
  styleUrls: ['./create-method-form.component.css']
})
export class CreateMethodComponent {
    @Input() title: string = '';

    createMethodForm: FormGroup;
    @Output() onSubmit = new EventEmitter<MethodCreateModel>();

    createMethodStatus: {
        loading?: true;
        error?: string;
    } | null = null;
    
    constructor(private fb: FormBuilder) {
        this.createMethodForm = this.fb.group({
            name: ['', [
                Validators.required,
                Validators.maxLength(30)
            ]],
            type: ['', [
                Validators.required
            ]],
        });
    }

    public onSubmitForm() {
        if (this.createMethodForm.invalid) {
            this.markAsTouched();
            return;
        }

        const formValue = this.createMethodForm.value;
        const newMethod: MethodCreateModel = {
            name: formValue.name,
            type: formValue.type,
            accessibility: formValue.accessibility ?? 'Public',
            isAbstract: formValue.isAbstract ?? false,
            isSealed: formValue.isSealed ?? false,
            isOverride: formValue.isOverride ?? false,
            isVirtual: formValue.isVirtual ?? false,
            classId: formValue.classId ?? '',
            localVariables: [],
            parameters: []
        };

        this.onSubmit.emit(newMethod);
    }

    private markAsTouched() {
        Object.values(this.createMethodForm.controls).forEach(control => {
            control.markAsTouched();
        });
    }
}