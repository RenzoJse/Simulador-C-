import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MethodDropdownComponent } from '../../../business-components/method/dropdown/method-dropdown.component';
import { MethodService } from '../../../backend/services/method/method.service';

@Component({
  selector: 'app-delete-method',
  standalone: true,
  imports: [ CommonModule, MethodDropdownComponent ],
  templateUrl: './delete-method.component.html'
})
export class DeleteMethodComponent implements OnInit {
  selectedMethodId: string | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private methodService: MethodService,
    private router: Router
  ) {}

  ngOnInit() {
  }

  onSelectionChange(id: string) {
    this.selectedMethodId = id;
    this.error = null;
  }

  onDeleteClicked() {
    if (!this.selectedMethodId) {
      this.error = 'You must select a method to delete.';
      return;
    }

    this.loading = true;
    this.error = null;

    this.methodService.deleteMethod(this.selectedMethodId).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/']);
      },
      error: err => {
        this.loading = false;
        this.error = err.message || 'Failed to delete method.';
      }
    });
  }
}