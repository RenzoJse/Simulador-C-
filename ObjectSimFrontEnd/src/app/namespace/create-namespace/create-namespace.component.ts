import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { NamespaceService } from '../../../backend/services/namespace/namespace.service';
import CreateNamespaceModel from '../../../backend/services/namespace/model/create-namespace.model';
import { CommonModule } from '@angular/common';
import { CreateNamespaceFormComponent } from '../../../business-components/namespace/create-namespace-form/create-namespace-form.component';

@Component({
  selector: 'app-create-namespace',
  templateUrl: './create-namespace.component.html',
  standalone: true,
  imports: [CommonModule, CreateNamespaceFormComponent],
  styleUrl: './create-namespace.component.css'
})
export class CreateNamespaceComponent {
  status: { loading?: boolean; error?: string } | null = null;
   createdNamespace: CreateNamespaceModel | null = null;

  constructor(
    private readonly _router: Router,
    @Inject(NamespaceService) private readonly _namespaceService: NamespaceService
  ) {}

  onSubmit(model: CreateNamespaceModel) {
    this.status = { loading: true };
    this.createdNamespace = null;
    this._namespaceService.createNamespace(model).subscribe({
      next: (response: CreateNamespaceModel) => {
        this.status = null;
        this.createdNamespace = response;
        alert('Namespace creado satisfactoriamente ✅');
        this._router.navigate(['/namespaces/create']);
      },
      error: (err) => {
        this.status = { error: err.error?.message || 'Failed to create namespace' };
        console.error(err);
      }
    });
  }
}
