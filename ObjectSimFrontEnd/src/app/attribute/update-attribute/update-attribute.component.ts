import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AttributeService } from '../../../backend/services/attribute/attribute.service';
import CreateAttributeModel from '../../../backend/services/attribute/models/create-attribute.model';
import { UpdateAttributeFormComponent } from "../../../business-components/attribute/update-attribute-form/update-attribute-form.component";
import { FormsModule, NgModel } from '@angular/forms';
import { CommonModule, NgIf } from '@angular/common';
@Component({
  selector: 'app-update-attribute',
  standalone: true,
  templateUrl: './update-attribute.component.html',
  styles: [],
  imports: [NgIf,FormsModule,UpdateAttributeFormComponent,CommonModule]
})
export class UpdateAttributeComponent implements OnInit {
  selectedAttributeId: string | null = null;
  attributeToEdit: CreateAttributeModel | null = null;
  allAttributes: { value: string, tag: string }[] = [];
  status: { loading?: boolean; error?: string } | null = null;

  constructor(
    private readonly router: Router,
    private readonly attributeService: AttributeService
  ) {}

  ngOnInit(): void {
    this.attributeService.getAllAttributes().subscribe({
      next: (attributes) => {
        this.allAttributes = attributes.map(attr => ({
          value: attr.classId,
          tag: attr.name
        }));
      },
      error: () => {
        this.status = { error: 'Error loading attributes' };
      }
    });
  }

  loadAttribute(): void {
    if (!this.selectedAttributeId) return;

    this.attributeService.getAttributeById(this.selectedAttributeId).subscribe({
      next: (attribute) => {
        this.attributeToEdit = attribute;
        this.status = null;
      },
      error: () => {
        this.status = { error: 'Failed to load attribute details' };
      }
    });
  }

  onUpdateSubmit(event: { id: string; model: CreateAttributeModel }): void {
    this.status = { loading: true };
    this.attributeService.updateAttribute(event.id, event.model).subscribe({
      next: () => {
        alert('Attribute updated successfully');
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.status = { error: err.message || 'Update failed' };
      }
    });
  }
}
