import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-delete-class-form',
  standalone: true,
  imports: [ CommonModule, ReactiveFormsModule, NgIf ],
  templateUrl: './delete-class-form.component.html'
})
export class DeleteClassFormComponent {
  @Input() classes: { id: string; name: string }[] = [];
  @Input() loading = false;
  @Input() error: string | null = null;
  @Output() atDelete = new EventEmitter<string>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      classId: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.atDelete.emit(this.form.value.classId);
  }
}