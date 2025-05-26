import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormComponent } from '../../components/form/form/form.component';

@Component({
    selector: 'app-create-class-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent],
    templateUrl: './create-class-form.component.html'
})

export class CreateClassFormComponent {
    @Input() title: string = '';

    createClassForm: FormGroup;
    @Output() atSubmit = new EventEmitter<any>();

    createClassStatus: {
        loading?: true;
        error?: string;
    } | null = null;

    ClassTypes: { value: string; tag: string }[] = [
        { value: 'Abstract', tag: 'Abstract' },
        { value: 'Interface', tag: 'Interface' },
        { value: 'Sealed', tag: 'Sealed' }
    ];

    constructor(private fb: FormBuilder) {
        this.createClassForm = this.fb.group({
            Name: ['', [
                Validators.required,
                Validators.maxLength(20),
                Validators.minLength(3)
            ]],
            ClassTypes: ['', [Validators.required]],
        });
    }

    public onSubmit() {
        console.log('Form values:', this.createClassForm.value);

        if (this.createClassForm.invalid) {
            this.markAsTouched();
            return;
        }

        const selectedType = this.createClassForm.value.ClassTypes;
        var formValue = this.createClassForm.value;
        var newClass = {
            Name: formValue.Name,
            Accesibility: '',
            IsAbstract: selectedType === 'Abstract',
            IsSealed: selectedType === 'Sealed',
            IsVirtual: selectedType === 'Virtual',
            Attributes: [],
            Methods: [],
            Parent: '',
        };
        
        this.atSubmit.emit(this.createClassForm.value);
    }

    private markAsTouched() {
        Object.keys(this.createClassForm.controls).forEach(field => {
            const control = this.createClassForm.get(field);
            if (control) {
                control.markAsTouched({ onlySelf: true });
            }
        });
    }
}