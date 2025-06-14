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
  imports: [
    ReactiveFormsModule,
    NgIf,
    FormComponent,
    FormInputComponent,
    FormButtonComponent,
    ClassDropdownComponent
  ],
  templateUrl: './create-method-form.component.html',
  styleUrls: ['./create-method-form.component.css']
})
export class CreateMethodFormComponent {
  @Input() title = '';
  @Output() atSubmit = new EventEmitter<MethodCreateModel>();

  createMethodForm: FormGroup;
  createMethodStatus: { loading?: true; error?: string } | null = null;

  AccessibilityOptions = [
    { value: 'Public', tag: 'Public' },
    { value: 'Private', tag: 'Private' },
    { value: 'Protected', tag: 'Protected' }
  ];

  Modificadores = [
    { value: 'Abstract', tag: 'Abstract' },
    { value: 'Sealed', tag: 'Sealed' },
    { value: 'Override', tag: 'Override' },
    { value: 'Virtual', tag: 'Virtual' },
    { value: 'Static', tag: 'Static' }
  ];

  selectedClassId = '';

  constructor(private fb: FormBuilder) {
    this.createMethodForm = this.fb.group({
      Name: ['', [Validators.required, Validators.maxLength(50)]],
      typeId: ['', [Validators.required]],
      ClassID: ['', [Validators.required]],
      Modificadores: ['', [Validators.required]],
      accessibility: ['Public']
    });
  }

  onClassSelected(event: { classId?: string }) {
    this.selectedClassId = event.classId ?? '';
    this.createMethodForm.get('ClassID')?.setValue(this.selectedClassId);
  }

  onSubmit() {
    console.log('Form values:', this.createMethodForm.value);

    if (this.createMethodForm.invalid) {
      this.createMethodForm.markAllAsTouched();
      return;
    }

    const fv = this.createMethodForm.value;
    const newMethod: MethodCreateModel = {
      name: fv.Name,
      type: fv.typeId,
      accessibility: fv.accessibility,
      isAbstract: fv.Modificadores === 'Abstract',
      isSealed: fv.Modificadores === 'Sealed',
      isOverride: fv.Modificadores === 'Override',
      isVirtual: fv.Modificadores === 'Virtual',
      isStatic: fv.Modificadores === 'Static',
      classId: this.selectedClassId,
      localVariables: [],
      parameters: []
    };

    this.atSubmit.emit(newMethod);
  }
}