import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule, NgIf }      from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-delete-method-form',
  standalone: true,
  imports: [ CommonModule, ReactiveFormsModule, NgIf ],
  templateUrl: './delete-method-form.component.html'
})
export class DeleteMethodFormComponent {
  @Input() methods: { id: string; name: string }[] = [];
  @Input() loading = false;
  @Input() error: string | null = null;
  @Output() atDelete = new EventEmitter<string>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      methodId: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.atDelete.emit(this.form.value.methodId);
  }
}