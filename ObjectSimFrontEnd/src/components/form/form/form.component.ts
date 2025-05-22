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
  @Output() atSend = new EventEmitter<any>();

  public localSend() {
    const isValid = this.form.valid

    if (isValid) {
      this.atSend.emit(this.form.value);
    } else {
      this.form.markAllAsTouched();
    }
  }
}
