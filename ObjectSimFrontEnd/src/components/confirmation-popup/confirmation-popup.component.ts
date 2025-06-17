import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-confirmation-popup',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './confirmation-popup.component.html',
    styleUrls: ['./confirmation-popup.component.css']
})
export class ConfirmationPopupComponent {
    @Input() show: boolean = false;
    @Input() itemName: string = '';
    @Input() message: string = 'Are you sure you want to continue?';

    @Output() confirm = new EventEmitter<void>();
    @Output() cancel = new EventEmitter<void>();

    onConfirm(): void {
        this.confirm.emit();
    }

    onCancel(): void {
        this.cancel.emit();
    }
}