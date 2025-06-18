import { Component, Input, OnDestroy, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from "rxjs";

import { ClassService } from '../../../backend/services/class/class.service';
import { DropdownComponent } from '../../../components/dropdown/dropdown.component';

import SystemClassesStatus from './models/SystemClassesStatus';

@Component({
  selector: 'app-class-dropdown',
  standalone: true,
  imports: [DropdownComponent, CommonModule],
  templateUrl: './class-dropdown.component.html',
})

export class ClassDropdownComponent implements OnInit, OnDestroy{
  @Input() value: string | null = null;
  @Output() selectClass = new EventEmitter<{ classId: string | undefined; }>();

  status: SystemClassesStatus = {
    loading: true,
    systemClasses: [],
    error: '',
  };

  private _everyoneStatus: Subscription | null = null;

  constructor(private readonly _classService: ClassService) {}
  ngOnDestroy(): void {
    this._everyoneStatus?.unsubscribe();
  }

  ngOnInit(): void {
    this._classService.getAllClasses()
        .subscribe({
          next: (systemClasses) => {
            this.status = {
              systemClasses: systemClasses.map((classObj) => ({
                value: classObj.id,
                tag: classObj.name,
              })),
            };
          },
          error: (error) => {
            this.status = { systemClasses: [], error: 'No available classes.' };
          }
        });
  }

  onSelectClass(classId: string) {
    const classObj = this.status.systemClasses.find(c => c.value === classId);
    if (classObj) {
      this.selectClass.emit({
        classId: classObj.value,
      });
    }
  }

}
