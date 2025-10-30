import {Component, EventEmitter, Input, Output} from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";

import { InputComponent } from "../../input/input.component";

@Component({
    selector: 'app-form-input',
    templateUrl: './form-input.component.html',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, InputComponent]
})
export class FormInputComponent {
    @Input() type: "text" | "number" | "case" | "option" = "text";
    @Input() tag: string | null = null;
    @Input() reserve: string | null = null;
    @Input({ required: true }) name!: string;
    @Input({ required: true }) form!: FormGroup;
    @Input() options: { value: string; tag: string }[] = [];

    @Input() controlName!: string;
    @Input() errorMessages: { [key: string]: string } = {};

    @Output() changeValue = new EventEmitter<string>();

    get control() {
        return this.form.get(this.controlName);
    }

    get errorMessage() {
        if (this.control?.errors) {
            for (const key in this.control.errors) {
                if (this.control.errors.hasOwnProperty(key)) {
                    return this.errorMessages[key];
                }
            }
        }
        return null;
    }

    get value() {
        return this.form.get(this.name)?.value;
    }

    set value(valor: string) {
        const control = this.form.get(this.name);
        if (control) {
            control.setValue(valor);
        }
    }

    changeOnValue(value: string) {
        const control = this.form.get(this.name);
        if (control) {
            control.setValue(value);
            this.changeValue.emit(value);
        }
    }
}
