import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf, CommonModule } from '@angular/common';
import { Router } from '@angular/router';

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
    showManualForm = false;
    successMessage: string | null = null;
    
    insertKeyStatus: {
        loading?: true;
        error?: string;
    } | null = null;
    
    constructor(private router: Router, private fb: FormBuilder) {
        this.insertKeyForm = this.fb.group({
            Key: ['', [Validators.required]],
        });
    }

    showAttributeForm = false;
    showMethodForm = false;

    public onSubmit() {
        if (this.insertKeyForm.valid) {
            this.insertKeyStatus = { loading: true };
            localStorage.setItem('key', this.insertKeyForm.value.Key);
            this.successMessage = 'Key added successfully!';
            this.insertKeyStatus = null;
            setTimeout(() => {
                this.router.navigate(['/']);
            }, 1000);
        } else {
            this.markAsTouched();
            this.insertKeyStatus = { error: 'Please fill in all required fields.' };
        }
    }

    generateAutomaticKey() {
        const guid = self.crypto.randomUUID();
        this.insertKeyForm.patchValue({ Key: guid });
        this.onSubmit();
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