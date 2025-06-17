import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-load-example-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './load-example-card.component.html',
  styleUrls: ['./load-example-card.component.css']
})
export class LoadExampleCardComponent {
  @Input() title = 'Load Example';
  @Input() buttonText = 'Load';
  @Input() successMessage = 'Loaded successfully!';
  @Input() errorMessage = 'Failed to load.';
  @Input() status: 'idle' | 'loading' | 'success' | 'error' = 'idle';

  @Output() onClick = new EventEmitter<void>();
}
