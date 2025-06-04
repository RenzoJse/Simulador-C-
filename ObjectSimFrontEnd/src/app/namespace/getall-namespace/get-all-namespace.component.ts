import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NamespaceService } from '../../../backend/services/namespace/namespace.service';
import CreateNamespaceModel from '../../../backend/services/namespace/model/create-namespace.model';

@Component({
  selector: 'app-get-all-namespaces',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './get-all-namespace.component.html',
})
export class GetAllNamespacesComponent implements OnInit {
  private readonly _namespaceService = inject(NamespaceService);
  namespaces: CreateNamespaceModel[] = [];
  status: { loading?: boolean; error?: string } | null = null;

  ngOnInit(): void {
    this.status = { loading: true };
    this._namespaceService.getAllNamespaces().subscribe({
      next: (data) => {
        this.namespaces = data;
        this.status = null;
      },
      error: (err) => {
        this.status = { error: err.error?.message || 'Failed to load namespaces' };
        console.error(err);
      }
    });
  }
}
