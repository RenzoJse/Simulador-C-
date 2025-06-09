import { Component, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { ClassService } from "../../../backend/services/class/class.service";


@Component({
    selector: 'app-delete-class',
    templateUrl: './delete-class.component.html'
})

export class DeleteClassComponent {

    status: { loading?: true; error?: string } | null = null;

    constructor(
        @Inject(ClassService) private readonly _classService : ClassService
    ) {
        console.log('DeleteClass inicializado');
    }

    protected atSubmit(classId: string) {
        this.status = { loading: true };
        console.log("Tying to delete class with ID:", classId);
        this._classService.deleteClass(classId).subscribe({
            next: (response) => {
                this.status = null;
            },
            error: (error:any) => {
                if (error.status === 400 && error.Message) {
                    this.status = { error: error.Message };
                } else {
                    this.status = { error: error.message || 'Error in simulation.' };
                }
            },
        });
    }
}