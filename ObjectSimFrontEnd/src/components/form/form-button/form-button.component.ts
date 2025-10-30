import { Component, Input } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";

@Component({
    selector: "app-form-button",
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: "./form-button.component.html",
    styles: ``,
})
export class FormButtonComponent {
    @Input({ required: true }) title!: string;
}

