import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import { FormInputComponent } from '../../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../../components/form/form-button/form-button.component';
import { FormComponent } from '../../../components/form/form/form.component';
import { ClassDropdownComponent } from '../../class/dropdown/class-dropdown.component';

import MethodCreateModel from '../../../backend/services/method/models/method-dto.model';

@Component({
  selector: 'app-create-method-form',
  standalone: true,
  imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent, ClassDropdownComponent],
  templateUrl: './create-method-form.component.html',
  styleUrls: ['./create-method-form.component.css']
})
export class CreateMethodFormComponent {
    @Input() title: string = '';
    @Input() classOptions: { id: string, name: string }[] = [];
    createMethodForm: FormGroup;
    @Output() atSubmit = new EventEmitter<MethodCreateModel>();

    createMethodStatus: {
        loading?: true;
        error?: string;
    } | null = null;

    AccessibilityOptions: { value: string; tag: string }[] = [
        { value: 'Public', tag: 'Public' },
        { value: 'Private', tag: 'Private' },
        { value: 'Protected', tag: 'Protected' }
    ];

    Modificadores: { value: string; tag: string }[] = [
        { value: 'Abstract', tag: 'Abstract' },
        { value: 'Sealed', tag: 'Sealed' },
        { value: 'Override', tag: 'Override' },
        { value: 'Virtual', tag: 'Virtual' }
    ];
    
    selectedClassId: string = '';

    constructor(private fb: FormBuilder) {
        this.createMethodForm = this.fb.group({
            Name: ['', [
                Validators.required,
                Validators.maxLength(30)
            ]],
            typeId: ['', [
                Validators.required
            ]],
            ClassID: ['', [
                Validators.required
            ]],
            Modificadores: ['', [Validators.required]],
            accessibility: ['Public']
        });
    }

    public onSubmit() {
        console.log('Form values:', this.createMethodForm.value);

        if (this.createMethodForm.invalid) {
            this.markAsTouched();
            return;
        }

        const selectedModifier = this.createMethodForm.value.Modificadores;

        const formValue = this.createMethodForm.value;
        const newMethod: MethodCreateModel = {
            name: formValue.Name,
            type: formValue.typeId,
            accessibility: formValue.accessibility ?? 'Public',
            isAbstract: selectedModifier === 'Abstract',
            isSealed: selectedModifier === 'Sealed',
            isOverride: selectedModifier === 'Override',
            isVirtual: selectedModifier === 'Virtual',
            classId: this.selectedClassId,
            localVariables: [],
            parameters: []
        };

        this.atSubmit.emit(newMethod);
    }

    private markAsTouched() {
        Object.values(this.createMethodForm.controls).forEach(control => {
            control.markAsTouched();
        });
    }

    onClassSelected(classId: string) {
        this.selectedClassId = classId;
    }
}