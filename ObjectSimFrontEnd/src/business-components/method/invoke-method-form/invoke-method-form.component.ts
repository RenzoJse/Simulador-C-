import {Component, Input, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf, CommonModule } from '@angular/common';

import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';
import { MethodDropdownComponent } from "../dropdown/method-dropdown.component";
import AddInvokeMethodModel from "../../../backend/services/method/models/add-invoke-method.model";

@Component({
    selector: 'app-add-invoke-method-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent,
        NgIf, FormComponent, CommonModule, MethodDropdownComponent],
    templateUrl: './invoke-method-form.component.html',
})

export class InvokeMethodFormComponent {
    @Input() title: string = '';
    @Output() atSubmit = new EventEmitter<AddInvokeMethodModel>();

    addInvokeMethodForm: FormGroup;
    InvokeMethodId: string | undefined;

    addInvokeMethodFormStatus: {
        loading?: true;
        error?: string;
    } | null = null;

    ReferenceTypes = [
        { value: 'this', tag: 'this'},
        { value: 'base', tag: 'base'},
    ];

    constructor(private fb: FormBuilder, private cdr: ChangeDetectorRef) {
        this.addInvokeMethodForm = this.fb.group({
            Reference: [''],
            InvokeMethodId: [''],
        });
    }

    public onSubmit() {
        console.log('Form values:', this.addInvokeMethodForm.value);
        if (this.addInvokeMethodForm.invalid) {
            this.markAsTouched();
            return;
        }

        this.atSubmit.emit(this.addInvokeMethodForm.value);
    }

    private markAsTouched() {
        Object.keys(this.addInvokeMethodForm.controls).forEach(field => {
            const control = this.addInvokeMethodForm.get(field);
            if (control) {
                control.markAsTouched({ onlySelf: true });
            }
        });
    }

    updateInvokeMethodId(event: { methodId: string | undefined; }) {
        this.InvokeMethodId = event.methodId;
        this.addInvokeMethodForm.patchValue({ InvokeMethodId: event.methodId });
        this.cdr.detectChanges();
    }

}