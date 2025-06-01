import { Component } from '@angular/core';
import { AttributeService } from '../../../backend/services/attribute/attribute.service';

@Component({
  selector: 'app-delete-attribute',
  templateUrl: './delete-attribute.component.html',
  styles: []
})
export class DeleteAttributeComponent {
  attributeId = '';
  status: { loading?: boolean; error?: string } | null = null;

  constructor(private attributeService: AttributeService) {}

  deleteAttribute(): void {
    if (!this.attributeId) return;

    this.status = { loading: true };

    this.attributeService.deleteAttribute(this.attributeId).subscribe({
      next: () => {
        alert('Attribute deleted successfully');
        this.status = null;
        this.attributeId = '';
      },
      error: (error) => {
        this.status = {
          error: error.error?.message || 'Failed to delete attribute'
        };
        console.error(error);
      }
    });
  }
}
