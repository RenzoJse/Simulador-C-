import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

import CreateAttributeModel from '../../../backend/services/attribute/models/create-attribute.model';

@Component({
    selector: 'app-create-attribute-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent],
    templateUrl: './create-attribute-form.component.html'
})

export class CreateAttributeFormComponent {
    @Input() tittle : string = '';

    createAttributeForm: FormGroup;
    @Output() atSubmit = new EventEmitter<CreateAttributeModel>();

    createAttributeStatus: {
        loading?: true;
        error?: string;
    } | null = null;

    Visibilities: { value: string; tag: string }[] = [
        { value: 'Public', tag: 'Public' },
        { value: 'Private', tag: 'Private' },
        { value: 'Protected', tag: 'Protected' }
    ];

    constructor(private formBuilder: FormBuilder) {
        this.createAttributeForm = this.formBuilder.group({
            name: ['', [Validators.required, Validators.maxLength(10)]],
            dataTypeID: ['', Validators.required],
            visibility: ['', Validators.required]
        });
    }

    public onSubmit() {
        console.log('Form attribute submitted:', this.createAttributeForm.value);

        if(this.createAttributeForm.invalid) {
            this.markAsTouched()
            return;
        }

        var formValue = this.createAttributeForm.value;
        var newAttribute: CreateAttributeModel = {
            name: formValue.name,
            dataTypeID: formValue.dataTypeID,
            visibility: formValue.visibility
        };

        this.atSubmit.emit(newAttribute);
    }

    private markAsTouched() {
        Object.values(this.createAttributeForm.controls).forEach(control => {
            control.markAsTouched();
        });
    }
}