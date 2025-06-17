import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VisitorApiRepository } from '../../../backend/repositories/visitor-api-repository.service';
import { LoadExampleCardComponent } from '../../../business-components/examples/load-example-card.component';

@Component({
  selector: 'app-visitor',
  standalone: true,
  imports: [CommonModule, LoadExampleCardComponent],
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
