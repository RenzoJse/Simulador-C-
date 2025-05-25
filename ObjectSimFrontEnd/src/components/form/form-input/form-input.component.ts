import {Component, EventEmitter, Input, Output} from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";

import { EntryComponent } from "../../entry/entry.component";

@Component({
    selector: 'app-form-entry',
    templateUrl: './form-entry.component.html',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, EntryComponent]
})
export class FormEntryComponent {
    @Input() type: "text" | "number" | "case" | "option" = "text";
    @Input() tag: string | null = null;
    @Input() reserve: string | null = null;
    @Input({ required: true }) name!: string;
    @Input({ required: true }) form!: FormGroup;
    @Input() options: { value: string; tag: string }[] = [];

    @Input() controlName!: string;
    @Input() errorMessage: { [key: string]: string } = {};

    @Output() changeValue = new EventEmitter<string>();

    get control() {
        return this.form.get(this.controlName);
    }

    get mensajeError() {
        if (this.control?.errors) {
            for (const key in this.control.errors) {
                if (this.control.errors.hasOwnProperty(key)) {
                    return this.errorMessage[key];
                }
            }
        }
        return null;
    }

    get value() {
        return this.form.get(this.name)?.value;
    }

    set valor(valor: string) {
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
