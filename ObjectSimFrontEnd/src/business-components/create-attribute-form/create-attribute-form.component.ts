import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormComponent } from '../../components/form/form/form.component';

import AttributeFormComponent from '../../backend/';

@Component({
    selector: 'app-create-attribute-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent],
    templateUrl: './create-attribute-form.component.html',
    styleUrls: ['./create-attribute-form.component.css']
})

export class CreateAttributeFormComponent {
    @Input() tittle : string = '';
    
    createAttributeForm: FormGroup;
    @Output() attributeCreated = new EventEmitter<void>();

    attributeForm: FormGroup;

    createAttributeStatus: {
        loading?: true;
        error?: string;
    } | null = null;

    Visibility: { value: string; tag: string }[] = [
        { value: 'Public', tag: 'Public' },
        { value: 'Private', tag: 'Private' },
        { value: 'Protected', tag: 'Protected' }
    ];

    constructor(private formBuilder: FormBuilder) {
        this.attributeForm = this.formBuilder.group({
            name: ['', [Validators.required, Validators.maxLength(10)]],
            dataTypeID: ['', Validators.required],
            accessibility: ['', Validators.required]
        });
    }

    public onSubmit() {
        console.log('Form attribute submitted:', this.attributeForm.value);
        
        if(this.createAttributeForm.invalid) {
            this.markAsTouched()
            return;
        }
        
        var selectedModifier = this.attributeForm.value.accessibility;
        var formValue = this.attributeForm.value;
        var newAttribute: AttributeCreateModel = {
            name: formValue.name,
            dataTypeID: formValue.dataTypeID,
            accessibility: selectedModifier
        };
        
        this.atSubmit.emit(newAttribute);
    }

    private markAsTouched() {
        Object.values(this.createAttributeForm.controls).forEach(control => {
            control.markAsTouched();
        });
    }
}