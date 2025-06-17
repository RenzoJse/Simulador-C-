import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VisitorApiRepository } from '../../../backend/repositories/visitor-api-repository.service';

@Component({
  selector: 'app-visitor',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './visitor.component.html',
  styleUrls: ['./visitor.component.css']
})
export class VisitorComponent {
  private readonly visitorService = inject(VisitorApiRepository);

  status: 'idle' | 'loading' | 'success' | 'error' = 'idle';

  loadExample() {
    this.status = 'loading';
    this.visitorService.loadExample().subscribe({
      next: () => this.status = 'success',
      error: () => this.status = 'error',
    });
  }
}
