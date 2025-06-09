import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { AttributeService } from '../../../backend/services/attribute/attribute.service';
import CreateAttributeModel from '../../../backend/services/attribute/models/create-attribute.model';
import { UpdateAttributeFormComponent } from "../../../business-components/attribute/update-attribute-form/update-attribute-form.component";

@Component({
  selector: 'app-update-attribute',
  standalone: true,
  templateUrl: './update-attribute.component.html',
  styles: [],
  imports: [NgIf,UpdateAttributeFormComponent],

})
export class UpdateAttributeComponent {
  status: { loading?: boolean; error?: string } | null = null;

  constructor(
    private router: Router,
    private attributeService: AttributeService
  ) {}

  onUpdateSubmit(event: { id: string; model: CreateAttributeModel }): void {
    this.status = { loading: true };

    this.attributeService.updateAttribute(event.id, event.model).subscribe({
      next: () => {
        this.status = null;
        alert('Attribute updated successfully');
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.status = { error: err.message || 'Failed to update attribute' };
        console.error(err);
      }
    });
  }
}
