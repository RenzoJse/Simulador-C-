import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';

import { DropdownComponent } from '../../../components/dropdown/dropdown.component';
import { NamespaceService } from '../../../backend/services/namespace/namespace.service';
import NamespaceDto from '../../../backend/services/namespace/model/namespace-dto.model';

@Component({
  selector: 'app-namespace-dropdown',
  standalone: true,
  imports: [CommonModule, DropdownComponent],
  templateUrl: './namespace-dropdown.component.html'
})
export class NamespaceDropdownComponent implements OnInit, OnDestroy {
  @Input() value: string | null = null;
  @Output() selectNamespace = new EventEmitter<{ namespaceId: string | undefined }>();

  status = {
    loading: true,
    error: '',
    options: [] as { value: string; tag: string }[]
  };

  private _sub: Subscription | null = null;

  constructor(private readonly _namespaceService: NamespaceService) {}

  ngOnInit(): void {
    this._sub = this._namespaceService.getAllNamespaces().subscribe({
      next: (namespaces: NamespaceDto[]) => {
        this.status.options = namespaces.map(ns => ({
          value: ns.id,
          tag: ns.name
        }));
        this.status.loading = false;
      },
      error: () => {
        this.status = {
          loading: false,
          error: 'Failed to load namespaces',
          options: []
        };
      }
    });
  }

  ngOnDestroy(): void {
    this._sub?.unsubscribe();
  }

  onSelect(value: string): void {
    this.selectNamespace.emit({ namespaceId: value });
  }
}
