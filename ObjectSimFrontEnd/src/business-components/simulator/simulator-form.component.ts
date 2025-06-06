import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
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
    @Output() atSubmit = new EventEmitter<CreateSimulatedExecutionModel>();

    simulatorForm: FormGroup;

    simulatorFormStatus: {
        loading?: true;
        error?: string;
    } | null = null;

    constructor(private fb: FormBuilder) {
        this.simulatorForm = this.fb.group({
        });
    }

    public onSubmit() {
        console.log('Form values:', this.simulatorForm.value);
    }

    private markAsTouched() {
        Object.values(this.simulatorForm.controls).forEach(control => {
            control.markAsTouched();
        });
    }
}