import { Component, Input, Output, EventEmitter } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { NgIf, CommonModule } from '@angular/common';

import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormComponent } from '../../components/form/form/form.component';

@Component({
    selector: 'app-insert-key-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent,
        NgIf, FormComponent, CommonModule],
    templateUrl: './insert-key-form.component.html'
})

export class InsertKeyFormComponent {
    @Input() title: string = '';
    @Output() atSubmit = new EventEmitter<any>();

    insertKeyForm: FormGroup;
    
    insertKeyStatus: {
        loading?: true;
        error?: string;
    } | null = null;
    
    constructor(private fb: FormBuilder) {
        this.insertKeyForm = this.fb.group({
            Name: ['', [
                Validators.required,
                Validators.maxLength(20),
                Validators.minLength(3)
            ]],
            ClassType: ['', [Validators.required]],
            ParentClassID: ['', []],
        });
    }

    showAttributeForm = false;
    showMethodForm = false;
    methods: any[] = [];
    attributes: any[] = [];

    addMethod(method: any) {
        this.methods.push(method);
        console.log('Form values:', this.insertKeyForm.value);
        console.log('Metodos guardados:', this.methods);
        this.showMethodForm = false;
    }

    public onSubmit() {
    }

    private markAsTouched() {
        Object.keys(this.insertKeyForm.controls).forEach(field => {
            const control = this.insertKeyForm.get(field);
            if (control) {
                control.markAsTouched({ onlySelf: true });
            }
        });
    }
}