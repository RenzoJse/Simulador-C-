import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './form.component.html',
})
export class FormComponent {
  @Input({ required: true }) form!: FormGroup;
  @Output() atSubmit = new EventEmitter<any>();

  public localSend() {
    if (this.form.valid) {
      this.atSubmit.emit(this.form.value);
    } else {
      this.form.markAllAsTouched();
    }
  }
}
