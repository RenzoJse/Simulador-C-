import { Component, Input, Output, EventEmitter } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { NgIf, CommonModule } from '@angular/common';

import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormComponent } from '../../components/form/form/form.component';

import { CreateMethodFormComponent } from '../method/create-method-form/create-method-form.component';
import { CreateAttributeFormComponent } from '../attribute/create-attribute-form/create-attribute-form.component';

@Component({
    selector: 'app-create-class-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent,
        NgIf, FormComponent, CreateMethodFormComponent,
        CreateAttributeFormComponent, CommonModule],
    templateUrl: './create-class-form.component.html',
    styleUrls: ['./create-class-form.component.css']
})

export class CreateClassFormComponent {
    @Input() title: string = '';

    createMethodForm: FormGroup;
    createClassForm: FormGroup;
    createAttributeForm: FormGroup;

    @Output() atSubmit = new EventEmitter<any>();

    createClassStatus: {
        loading?: true;
        error?: string;
    } | null = null;

    ClassTypes: { value: string; tag: string }[] = [
        { value: 'Abstract', tag: 'Abstract' },
        { value: 'Interface', tag: 'Interface' },
        { value: 'Sealed', tag: 'Sealed' },
        { value: 'Virtual', tag: 'Virtual' }
    ];

    AccessibilityOptions = [
        { value: 'Public', tag: 'Public'},
        { value: 'Private', tag: 'Private'},
        { value: 'Protected', tag: 'Protected'}
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
            dataTypeID: new FormControl("", [Validators.required]),
            visibility: new FormControl("", [Validators.required]),
        });

        this.createAttributeForm = new FormGroup({
            name: new FormControl("", [Validators.required]),
            dataTypeID: new FormControl("", [Validators.required]),
            visibility: new FormControl("", [Validators.required])
        });
    }

    showAttributeForm = false;
    showMethodForm = false;
    methods: any[] = [];
    attributes: any[] = [];

    addMethod(method: any) {
        this.methods.push(method);
        console.log('Form values:', this.createMethodForm.value);
        console.log('Metodos guardados:', this.methods);
        this.showMethodForm = false;
    }

    addAttribute(attribute: any) {
        this.attributes.push(attribute);
        console.log('Atributos guardados:', this.attributes);
        this.showAttributeForm = false;
    }

    showAddAttributeForm() {
        this.showAttributeForm = !this.showAttributeForm;
    }

    showAddMethodForm() {
        this.showMethodForm = !this.showMethodForm;
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
            name: formValue.Name,
            isAbstract: formValue.ClassType === 'Abstract',
            isSealed:   formValue.ClassType === 'Sealed',
            isVirtual:  formValue.ClassType === 'Virtual',
            isInterface:  formValue.ClassType === 'Interface',
            attributes: this.attributes,
            methods:    this.methods,
            parent: formValue.ParentClassID || ''
        };

        this.atSubmit.emit(newClass);
        console.log('New class: ', newClass);
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