import { Component, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormComponent } from '../../components/form/form/form.component';
import { ClassDropdownComponent } from '../../business-components/class/dropdown/class-dropdown.component';
import { MethodDropdownComponent } from '../../business-components/method/dropdown/method-dropdown.component';

import CreateSimulatedExecutionModel from '../../backend/services/simulator/models/create-simulated-execution.model';

@Component({
    selector: 'app-simulator-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent,
        NgIf, FormComponent, ClassDropdownComponent,
        MethodDropdownComponent],
    templateUrl: './simulator-form.component.html'
})

export class SimulatorFormComponent {
    @Input() title: string = '';
    @Input() methods: { id: string; name: string }[] = [];
    @Output() atSubmit = new EventEmitter<CreateSimulatedExecutionModel>();

    simulatorForm: FormGroup;

    simulatorFormStatus: {
        loading?: true;
        error?: string;
    } | null = null;

    ReferenceId: string | undefined;
    InstanceId: string | undefined;

    constructor(private fb: FormBuilder, private cdr: ChangeDetectorRef) {
        this.simulatorForm = this.fb.group({
            ReferenceId: [''],
            InstanceId: [''],
            methodId: ['']
        });
    }

    public onSubmit() {
        if (this.simulatorForm.valid) {
            const {ReferenceId, InstanceId, methodId} = this.simulatorForm.value;

            console.log('Reference ID:', ReferenceId);
            console.log('Instance ID:', InstanceId);
            console.log('Método:', methodId);

            this.atSubmit.emit(this.simulatorForm.value as CreateSimulatedExecutionModel);
        } else {
            this.markAsTouched();
            console.log('Invalid form:', this.simulatorForm.errors);
        }
    }

    private markAsTouched() {
        Object.values(this.simulatorForm.controls).forEach(control => {
            control.markAsTouched();
        });
    }

    updateClassReferenceId(event: { classId: string | undefined; }) {
        this.ReferenceId = event.classId;
        this.simulatorForm.patchValue({ ReferenceId: event.classId });
        this.cdr.detectChanges();
    }

    updateClassInstanceId(event: { classId: string | undefined; }) {
        this.InstanceId = event.classId;
        this.simulatorForm.patchValue({ InstanceId: event.classId });
        this.cdr.detectChanges();
    }

    updateSelectedMethodId(id: string) {
        this.simulatorForm.patchValue({ methodId: id });
        this.cdr.detectChanges();
    }

}