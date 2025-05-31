import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule, NgIf, NgForOf } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

import { MethodService } from '../../../backend/services/method/method.service';

interface MethodListItem {
  id: string;
  name: string;
}

@Component({
  selector: 'app-method-dropdown',
  standalone: true,
  imports: [ CommonModule, ReactiveFormsModule, NgIf, NgForOf ],
  templateUrl: './method-dropdown.component.html'
})
export class MethodDropdownComponent implements OnInit {
  @Input() label: string = 'Select Method';

  @Input() initialValue: string | null = null;

  @Output() selectionChange = new EventEmitter<string>();

  methods: MethodListItem[] = [];

  form: FormGroup;

  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private methodService: MethodService
  ) {
    this.form = this.fb.group({
      methodId: [this.initialValue, Validators.required]
    });
  }

  ngOnInit() {
    this.loadMethods();
    this.form.get('methodId')!.valueChanges.subscribe((val: string) => {
      this.selectionChange.emit(val);
    });
  }

  private loadMethods() {
    this.loading = true;
    this.methodService.getMethods().subscribe({
      next: (list: MethodListItem[]) => {
        this.methods = list;
        this.loading = false;
        if (this.initialValue) {
          this.form.get('methodId')!.setValue(this.initialValue);
        }
      },
      error: err => {
        this.error = err.message || 'Could not load methods.';
        this.loading = false;
      }
    });
  }

  public isValid(): boolean {
    return this.form.valid;
  }
}