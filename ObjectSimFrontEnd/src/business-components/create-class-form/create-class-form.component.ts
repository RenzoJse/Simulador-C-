import { Component, Input, Output, EventEmitter } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormComponent } from '../../components/form/form/form.component';

import { CreateMethodFormComponent } from '../../business-components/create-method-form/create-method-form.component'

@Component({
    selector: 'app-create-class-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent, CreateMethodFormComponent],
    templateUrl: './create-class-form.component.html'
})

export class CreateClassFormComponent {
    @Input() title: string = '';

    createMethodForm: FormGroup;
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
            ClassType: ['', [Validators.required]],
            ParentClassID: ['', []],
        });
        this.createMethodForm = new FormGroup({
            name: new FormControl("", [Validators.required]),
            typeID: new FormControl("", [Validators.required]),
        });
    }

    showMethodForm = false;
    methods: any[] = [];

    addMethod(method: any) {
        this.methods.push(method);
        console.log('Form values:', this.createMethodForm.value);
        console.log('Metodos guardados:', this.methods);
        this.showMethodForm = false;
    }

    showAddMethodForm() {
        this.showMethodForm = true;
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
            Methods: this.methods,
            Parent: '',
        };
        
        this.atSubmit.emit(newClass);
        console.log('Nueva Clase: ', newClass);
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