import { Component, Input, OnDestroy, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from "rxjs";

import { DropdownComponent } from '../../../components/dropdown/dropdown.component';
import { MethodService } from '../../../backend/services/method/method.service';
import SystemMethodStatus from './models/SystemMethodStatus';

@Component({
  selector: 'app-method-dropdown',
  standalone: true,
  imports: [DropdownComponent, CommonModule],
  templateUrl: './method-dropdown.component.html'
})

export class MethodDropdownComponent implements OnInit {
  @Input() value: string | null = null;
  @Output() selectMethod = new EventEmitter<{ methodId: string | undefined; }>();

  status: SystemMethodStatus = {
    loading: true,
    systemMethods: [],
    error: '',
  };

  private _everyoneStatus: Subscription | null = null;

  constructor(private readonly _methodService: MethodService) {}
  ngOnDestroy(): void {
    this._everyoneStatus?.unsubscribe();
  }

  ngOnInit(): void {
    this._methodService.getAllMethods()
        .subscribe({
          next: (systemMethods) => {
            this.status = {
              systemMethods: systemMethods.map((methods) => ({
                value: methods.id,
                tag: methods.name,
              })),
            };
          },
          error: (error) => {
            this.status = { systemMethods: [], error: 'No available methods.' };
          }
        });
  }

  onSelectMethod(methodId: string) {
    const method = this.status.systemMethods.find(m => m.value === methodId);
    if (method) {
      this.selectMethod.emit({
        methodId: method.value,
      });
    }
  }

}