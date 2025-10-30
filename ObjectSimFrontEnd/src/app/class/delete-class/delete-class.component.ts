import { Component, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { ClassService } from "../../../backend/services/class/class.service";

@Component({
    selector: 'app-delete-class',
    templateUrl: './delete-class.component.html'
})

export class DeleteClassComponent {

    status: { loading?: true; error?: string } | null = null;
    success = false;

    constructor(
        @Inject(ClassService) private readonly _classService : ClassService
    ) {
    }

    protected atSubmit(classId: string) {
        this.success = false;
        this.status = { loading: true };
        this._classService.deleteClass(classId).subscribe({
            next: (response) => {
                this.status = null;
                this.success = true;
                setTimeout(() => this.success = false, 5000);
            },
            error: (error:any) => {
                if (error.status === 400 && error.Message) {
                    this.status = { error: error.Message };
                } else {
                    this.status = { error: error.message || 'Error in deleting class.' };
                }
            },
        });
    }
}