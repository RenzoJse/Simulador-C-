import { Component, OnInit } from '@angular/core';
import { AttributeService } from '../../../backend/services/attribute/attribute.service';
import CreateAttributeModel from '../../../backend/services/attribute/models/create-attribute.model';
import AttributeUpdateModel from '../../../backend/services/attribute/models/attribute-update.model';

@Component({
  selector: 'app-delete-attribute',
  templateUrl: './delete-attribute.component.html',
  styles: []
})
export class DeleteAttributeComponent implements OnInit {
  attributeId = '';
  attributesDropdown: { value: string; tag: string }[] = [];
  status: { loading?: boolean; error?: string } | null = null;

  constructor(private attributeService: AttributeService) {}

  ngOnInit(): void {
    this.attributeService.getAllAttributes().subscribe({
      next: (attrs: AttributeUpdateModel[]) => {
        this.attributesDropdown = attrs.map(attr => ({
          value: attr.id,
          tag: `${attr.name} (${attr.visibility})`
        }));
      },
      error: () => {
        this.status = { error: 'Error loading attributes' };
      }
    });
  }

  deleteAttribute(): void {
    if (!this.attributeId) return;

    this.status = { loading: true };

    this.attributeService.deleteAttribute(this.attributeId).subscribe({
      next: () => {
        alert('Attribute deleted successfully');
        this.status = null;
        this.attributeId = '';
        this.ngOnInit();
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
