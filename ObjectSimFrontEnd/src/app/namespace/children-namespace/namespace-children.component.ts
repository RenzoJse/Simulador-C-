import { Component, OnInit } from '@angular/core';
import { NamespaceService } from '../../../backend/services/namespace/namespace.service';
import NamespaceDto from '../../../backend/services/namespace/model/namespace-dto.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-namespace-children',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './namespace-children.component.html'
})
export class NamespaceChildrenComponent implements OnInit {
  namespaces: NamespaceDto[] = [];
  selectedNamespaceId: string = '';
  descendants: NamespaceDto[] = [];
  error: string | null = null;

  constructor(private namespaceService: NamespaceService) {}

  ngOnInit(): void {
    this.namespaceService.getAllNamespaces().subscribe({
      next: (data) => this.namespaces = data,
      error: (err) => this.error = 'Error fetching namespaces'
    });
  }

  onSelect(): void {
    if (!this.selectedNamespaceId) return;

    this.namespaceService.getDescendants(this.selectedNamespaceId).subscribe({
      next: (data) => {
        this.descendants = data;
        this.error = null;
      },
      error: () => this.error = 'Error fetching descendants'
    });
  }
}
