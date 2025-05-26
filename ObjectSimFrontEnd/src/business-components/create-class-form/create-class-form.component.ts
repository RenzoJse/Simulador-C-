import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';

import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormComponent } from '../../components/form/form/form.component';

@Component({
    selector: 'app-create-class-form',
    standalone: true,
    imports: [ReactiveFormsModule, FormInputComponent, FormButtonComponent, NgIf, FormComponent],
    templateUrl: './create-class-form.component.html'
})