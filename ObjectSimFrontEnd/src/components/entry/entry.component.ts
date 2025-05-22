import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CommonModule } from '@angular/common';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
    selector: "app-entry",
    templateUrl: "./entry.component.html",
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
})
export class EntryComponent {
    @Input() type: "text" | "number" | "case" | "option" = "text";
    @Input() form!: FormGroup;
    @Input() tag: string | null = null;
    @Input() reserve: string | null = null;
    @Input() value: string | null = null;
    @Input() options: { value: string; tag: string }[] = [];
    @Input() controlName!: string;
    @Input() invalid: boolean = false;

    @Output() valueChange = new EventEmitter<string>(); //

    public showPassword: boolean = false;

    public inValueChange(event: any): void { //
        this.valueChange.emit(event.target.value); //
    }
}
