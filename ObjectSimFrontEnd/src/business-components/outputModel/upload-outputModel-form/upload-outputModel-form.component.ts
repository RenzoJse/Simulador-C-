import { Component, Input, Output, EventEmitter } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-upload-outputModel-form',
    standalone: true,
    imports: [ReactiveFormsModule, NgIf],
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

    constructor(private fb: FormBuilder) {
        this.uploadOutputModelForm = this.fb.group({
            filename: ['', Validators.required],
        });
    }

    public onSubmit() {
            if (!this.uploadedModelFile) {
                this.uploadOutputModelFormStatus = { error: 'You need to select a .dll file.' };
                return;
            }else{
                const { filename } = this.uploadOutputModelForm.value;

                console.log('Filename:', filename);

                this.atSubmit.emit({
                    filename,
                    file: this.uploadedModelFile
                });
            }
    }

    public onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.uploadedModelFile = input.files[0];
            console.log('Select File:', this.uploadedModelFile.name);
            console.log('File size:', this.uploadedModelFile.size, 'bytes');
        }
    }

}