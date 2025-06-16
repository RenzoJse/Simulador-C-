import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dropdown-update',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <select class="form-control"
            [ngModel]="selectedId"
            (ngModelChange)="onChange($event)">
      <option value="" disabled>Select an option</option>
      <option *ngFor="let item of items" [value]="item[bindValue]">
        {{ item[bindLabel] }}
      </option>
    </select>
  `
})
export class DropdownUpdateComponent {
  @Input() items: any[] = [];
  @Input() bindLabel: string = 'name';
  @Input() bindValue: string = 'id';
  @Input() selectedId: string | null = null;
  @Output() selectedIdChange = new EventEmitter<string>();
  @Output() selected = new EventEmitter<string>();

  onChange(value: string): void {
    this.selectedIdChange.emit(value);
    this.selected.emit(value);
  }
}
