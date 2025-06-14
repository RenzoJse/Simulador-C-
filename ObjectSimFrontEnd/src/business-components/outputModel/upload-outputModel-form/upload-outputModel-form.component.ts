import { Component, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';

@Component({
    selector: 'app-upload-outputModel-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormButtonComponent,
        NgIf, FormComponent],
    templateUrl: './upload-outputModel-form.component.html'
})

export class UploadOutputModelFormComponent {
    @Input() title: string = '';
    @Input() uploadedModelFile: File | null = null;
    @Output() atSubmit = new EventEmitter<{ filename: string, file: File | null }>();

    uploadOutputModelForm: FormGroup;

    uploadOutputModelFormStatus: {
        loading?: true;
        error?: string;
    } | null = null;

    constructor(private fb: FormBuilder, private cdr: ChangeDetectorRef) {
        this.uploadOutputModelForm = this.fb.group({
            filename: ['', Validators.required],
        });
    }

    public onSubmit() {
        if (this.uploadOutputModelForm.valid) {

            if (!this.uploadedModelFile) {
                this.uploadOutputModelFormStatus = { error: 'Debes seleccionar un archivo.' };
                return;
            }

            const { filename } = this.uploadOutputModelForm.value;

            console.log('Filename:', filename);

            this.atSubmit.emit(this.uploadOutputModelForm.value);

        } else {
            this.markAsTouched();
            console.log('Invalid form:', this.uploadOutputModelForm.errors);
        }
    }

    public onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.uploadedModelFile = input.files[0];
            console.log('Select File:', this.uploadedModelFile.name);
        }
    }

    private markAsTouched() {
        Object.values(this.uploadOutputModelForm.controls).forEach(control => {
            control.markAsTouched();
        });
    }

}