import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule, NgIf } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

import CreateAttributeModel from '../../../backend/services/attribute/models/create-attribute.model';

@Component({
    selector: 'app-create-attribute-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent,CommonModule],
    templateUrl: './create-attribute-form.component.html'
})

export class CreateAttributeFormComponent {
    @Input() tittle : string = '';
    @Input() availableClasses: { value: string; tag: string }[] = [];
    @Input() availableDataTypes: { value: string; tag: string }[] = [];
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

    Static: { value: string; tag: string }[] = [
        { value: "true", tag: 'Static' },
        { value: "false", tag: 'Non-Static' }
    ];

    private classId: string = '';

    constructor(private formBuilder: FormBuilder, private route: ActivatedRoute) {
        this.createAttributeForm = this.formBuilder.group({
            name: ['', [Validators.required,
                Validators.maxLength(10),
                Validators.minLength(1)]],
            dataTypeID: ['', Validators.required],
            visibility: ['', Validators.required],
            isStatic: [false, Validators.required],
            classId: ['', Validators.required],
        });

        this.route.paramMap.subscribe(params => {
            this.classId = params.get('classId') ?? '';
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
            visibility: formValue.visibility,
            dataTypeId: formValue.dataTypeID,
            classId: this.classId,
            isStatic: formValue.isStatic === "true"
        };

        this.atSubmit.emit(newAttribute);
        console.log('try to create new attribute: ', newAttribute);
    }

    private markAsTouched() {
        Object.values(this.createAttributeForm.controls).forEach(control => {
            control.markAsTouched();
        });
    }
}