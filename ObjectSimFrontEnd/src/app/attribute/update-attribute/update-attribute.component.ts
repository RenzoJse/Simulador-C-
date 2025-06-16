import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AttributeService } from '../../../backend/services/attribute/attribute.service';
import AttributeUpdateModel from '../../../backend/services/attribute/models/attribute-update.model';
import { UpdateAttributeFormComponent } from "../../../business-components/attribute/update-attribute-form/update-attribute-form.component";
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DropdownUpdateComponent } from '../../../business-components/attribute/dropUpdate/dropdown-update.component';

@Component({
  selector: 'app-update-attribute',
  standalone: true,
  templateUrl: './update-attribute.component.html',
  imports: [FormsModule, CommonModule, UpdateAttributeFormComponent, DropdownUpdateComponent]
})
export class UpdateAttributeComponent implements OnInit {
  selectedAttributeId: string | null = null;
  attributeToEdit: AttributeUpdateModel | null = null;
  allAttributes: { id: string; name: string }[] = [];
  status: { loading?: boolean; error?: string } | null = null;

  constructor(
    private readonly router: Router,
    private readonly attributeService: AttributeService
  ) {}

  ngOnInit(): void {
    this.attributeService.getAllAttributes().subscribe({
      next: (attributes) => {
        this.allAttributes = attributes.map(attr => ({
          id: attr.id,
          name: attr.name
        }));
      },
      error: () => {
        this.status = { error: 'Error loading attributes' };
      }
    });
  }

  loadAttribute(id: string): void {
    this.selectedAttributeId = id;
    this.attributeService.getAttributeById(id).subscribe({
      next: (attribute) => {
        this.attributeToEdit = attribute;
        this.status = null;
      },
      error: () => {
        this.status = { error: 'Failed to load attribute details' };
      }
    });
  }

  onUpdateSubmit(attribute: AttributeUpdateModel): void {
    this.status = { loading: true };
    this.attributeService.updateAttribute(attribute.id, attribute).subscribe({
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
