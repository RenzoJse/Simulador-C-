import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ClassDropdownComponent } from '../../../business-components/class/dropdown/class-dropdown.component';
import { ClassService } from '../../../backend/services/class/class.service';

@Component({
  selector: 'app-delete-method',
  standalone: true,
  imports: [ CommonModule, ClassDropdownComponent ],
  templateUrl: './delete-class.component.html'
})
export class DeleteClassComponent implements OnInit {
  selectClassId: string | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private classService: ClassService,
    private router: Router
  ) {}

  ngOnInit() {
  }

  onSelectionChange(id: string) {
    this.selectClassId = id;
    this.error = null;
  }

  onDeleteClicked() {
    if (!this.selectClassId) {
      this.error = 'You must select a class to delete.';
      return;
    }

    this.loading = true;
    this.error = null;

    this.classService.deleteClass(this.selectClassId).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/']);
      },
      error: err => {
        this.loading = false;
        this.error = err.message || 'Failed to delete class.';
      }
    });
  }
}