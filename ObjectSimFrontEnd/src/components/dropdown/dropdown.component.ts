import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import DropdownOption  from "./models/option.component";

@Component({
    selector: 'app-dropdown',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './dropdown.component.html',
    styles: ``,
})

export class DropdownComponent implements OnInit {
    @Input({ required: true }) options!: Array<DropdownOption>;
    @Input() tag: string | null = null;
    @Input() reserve: string | null = null;
    @Input() emptyMessage = 'There are no options.';
    @Input() value: string | null = null;

    @Output() changeValue = new EventEmitter<string>();

    public ngOnInit(): void {
        if (!this.options || this.options.length === 0) {
            return;
        }

        if (!this.value && !this.reserve) {
            this.atChange({ target: { value: this.options[0].value } });
        }
    }

    public atChange(event: any) {
        const newOption =
            event.target.value === 'null' ? null : event.target.value;
        this.changeValue.emit(newOption);
    }
}