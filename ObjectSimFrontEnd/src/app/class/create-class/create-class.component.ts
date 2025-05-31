import { Component, Inject } from '@angular/core';
import { Router } from "@angular/router";
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { ClassService } from "../../../backend/services/class/class.service";
import CreateClassModel from "../../../backend/services/class/models/create-class.model";

@Component({
    selector: 'app-create-class',
    templateUrl: './create-class.component.html',
    styles: []
})

export class CreateClassComponent {
    createClassForm: FormGroup;
    status: { loading?: true; error?: string } | null = null;

    constructor(
        private readonly _router: Router,
        @Inject(ClassService) private readonly _classService: ClassService
    ) {
        this.createClassForm = new FormGroup({
            name: new FormControl("", [Validators.required]),
        });
    }

    protected atSubmit(classObj: CreateClassModel) {
        console.log('Formulario enviado:', classObj);
        this.status = { loading: true };

        this._classService.createClass(classObj).subscribe({
            next: (response) => {
                this.status = null;
                this._router.navigate([""]);
            },
            error: (error:any) => {
                if (error.status === 400 && error.Message) {
                    this.status = { error: error.Message };
                } else {
                    this.status = { error: error.message || 'Error creating class.' };
                }
            },
        });
    }
}