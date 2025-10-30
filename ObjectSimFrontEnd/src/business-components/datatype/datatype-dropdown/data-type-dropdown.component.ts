import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';

import { DataTypeService } from '../../../backend/services/datatype/datatype.service';
import { DropdownComponent } from '../../../components/dropdown/dropdown.component';
import DataTypeModel from '../../../backend/services/datatype/models/datatype.model';

@Component({
  selector: 'app-datatype-dropdown',
  standalone: true,
  imports: [DropdownComponent, CommonModule],
  templateUrl: './datatype-dropdown.component.html',
})
export class DataTypeDropdownComponent implements OnInit, OnDestroy {
  @Input() value: string | null = null;
  @Output() selectDataType = new EventEmitter<string>();

  status = {
    loading: true,
    error: '',
    options: [] as { value: string; tag: string }[],
  };

  private _sub: Subscription | null = null;

  constructor(private readonly _dataTypeService: DataTypeService) {}

  ngOnInit(): void {
    this._sub = this._dataTypeService.getAll().subscribe({
      next: (dataTypes) => {
        this.status.options = dataTypes.map(d => ({ value: d.id, tag: d.type }));
        this.status.loading = false;
      },
      error: () => {
        this.status = { loading: false, error: 'No datatypes found', options: [] };
      }
    });
  }

  ngOnDestroy(): void {
    this._sub?.unsubscribe();
  }

  onSelect(value: string) {
    this.selectDataType.emit(value);
  }
}
