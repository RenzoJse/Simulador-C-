import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClassService } from '../../../backend/services/class/class.service';
import ClassListItem from '../../../backend/services/class/models/class-list-item';

@Component({
  selector: 'app-multi-select',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './multi-select.component.html',
  styleUrls: ['./multi-select.component.css']
})
export class MultiSelectComponent implements OnInit {
  classes: ClassListItem[] = [];
  selectedIds: Set<string> = new Set();

  @Output() selectedClassIds = new EventEmitter<string[]>();

  constructor(private classService: ClassService) {}

  ngOnInit(): void {
    this.classService.getAllClasses().subscribe({
      next: (data) => this.classes = data,
      error: () => console.error('Failed to load classes')
    });
  }

  toggle(id: string): void {
    if (this.selectedIds.has(id)) {
      this.selectedIds.delete(id);
    } else {
      this.selectedIds.add(id);
    }

    this.selectedClassIds.emit([...this.selectedIds]);
  }
}
