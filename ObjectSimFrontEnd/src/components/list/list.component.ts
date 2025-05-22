import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';

@Component({
    selector: 'app-list',
    standalone: true,
    imports: [NgIf, NgFor],
    templateUrl: './list.component.html',
    styles: ``
})
export class ListComponent<T extends { [key: string]: any }> {
    @Input() elements: T[] = [];  // Recieves the elements to list
    @Input() columns: string[] = [];  // The columns that will be shown in the table
    @Output() selectedElement = new EventEmitter<T>();  // Event to select an element

    actualSelectedElement?: T;
    selectElement(element: T): void {
        if (this.actualSelectedElement === element) {
            this.actualSelectedElement = undefined;
            this.selectedElement.emit(undefined);
        } else {
            this.actualSelectedElement = element;
            this.selectedElement.emit(element);
        }
    }
}
