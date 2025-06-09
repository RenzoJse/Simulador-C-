import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClassService } from "../../../backend/services/class/class.service";
import { FormGroup } from '@angular/forms';
import ClassDtoOut from '../../../backend/services/class/models/class-dto-out.model';
import UpdateClassModel from '../../../backend/services/class/models/update-class.model';

@Component({
  selector: 'app-update-class',
  templateUrl: './update-class.component.html',
  styles: []
})
export class UpdateClassComponent implements OnInit {
  selectedClassId: string | null = null;
  selectedClass: ClassDtoOut | null = null;
  status: { loading?: true; error?: string } | null = null;

  constructor(
    @Inject(ClassService) private readonly _classService: ClassService,
    private readonly _router: Router,
    private readonly _route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.selectedClassId = this._route.snapshot.paramMap.get('id');
    if (this.selectedClassId) {
      this._classService.getById(this.selectedClassId).subscribe({
        next: (data) => {
          this.selectedClass = data;
        },
        error: (err) => {
          this.status = { error: 'Failed to load class.' };
        }
      });
    }
  }

  protected onUpdate(updatedData: UpdateClassModel) {
    if (!this.selectedClassId) return;
    this.status = { loading: true };

    this._classService.updateClass(this.selectedClassId, updatedData).subscribe({
      next: () => {
        this.status = null;
        this._router.navigate(['']);
      },
      error: (error: any) => {
        this.status = { error: error.message || 'Error updating class.' };
      }
    });
  }
}