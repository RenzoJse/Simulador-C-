import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import ClassDtoOut from '../../../backend/services/class/models/class-dto-out';
import UpdateClassModel from '../../../backend/services/class/models/update-class-model'; 
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-class-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './update-class-form.component.html'
})
export class UpdateClassFormComponent implements OnInit {
  @Input() classData!: ClassDtoOut;
  @Output() atSubmit = new EventEmitter<UpdateClassModel>();

  updateForm!: FormGroup;

  classTypes = ['Abstract', 'Interface', 'Sealed'];

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.updateForm = this.fb.group({
      name: [this.classData.name, [Validators.required, Validators.minLength(3)]],
      classType: [
        this.getClassType(this.classData),
        Validators.required
      ],
      parent: [this.classData.parent || '']
    });
  }

  getClassType(data: ClassDtoOut): string {
    if (data.isAbstract) return 'Abstract';
    if (data.isInterface) return 'Interface';
    if (data.isSealed) return 'Sealed';
    return '';
  }

  submit(): void {
    if (this.updateForm.invalid) return;

    const formValue = this.updateForm.value;
    const updateModel: UpdateClassModel = {
      name: formValue.name,
      isAbstract: formValue.classType === 'Abstract',
      isInterface: formValue.classType === 'Interface',
      isSealed: formValue.classType === 'Sealed',
      isVirtual: false,
      parent: formValue.parent || null
    };

    this.atSubmit.emit(updateModel);
  }
}