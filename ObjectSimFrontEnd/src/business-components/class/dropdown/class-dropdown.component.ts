import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule, NgIf, NgForOf } from '@angular/common';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { ClassService } from '../../../backend/services/class/class.service';
import ClassListItem from '../../../backend/services/class/models/class-list-item';

@Component({
  selector: 'app-class-dropdown',
  standalone: true,
  imports: [ CommonModule, ReactiveFormsModule, NgIf, NgForOf ],
  templateUrl: './class-dropdown.component.html'
})
export class ClassDropdownComponent implements OnInit {
  @Input() label: string = 'Select Class';
  @Input() form!: FormGroup;
  @Input() controlName!: string;
  @Input() initialValue: string = '';
  @Output() selectionChange = new EventEmitter<string>();

  classes: ClassListItem[] = [];
  loading = false;
  error: string | null = null;

  constructor(private classService: ClassService) {}

  ngOnInit() {
    this.loadClasses();

    this.form.get(this.controlName)?.valueChanges.subscribe((val: string) => {
      this.selectionChange.emit(val);
    });
  }

  private loadClasses() {
    this.loading = true;
    this.error = null;

    this.classService.getAllClasses().subscribe({
      next: (list: ClassListItem[]) => {
        this.classes = list;
        this.loading = false;
        if (this.initialValue) {
          this.form.get(this.controlName)?.setValue(this.initialValue);
        }
      },
      error: err => {
        this.loading = false;
        this.error = err.message || 'Could not load classes.';
      }
    });
  }
}