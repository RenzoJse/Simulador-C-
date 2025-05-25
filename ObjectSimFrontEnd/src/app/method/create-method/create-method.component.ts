import { Component, Inject } from '@angular/core';
import { Router } from "@angular/router";
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { MethodService } from "../../../backend/services/method/method.service";
import MethodCreateModel from "../../../backend/services/method/models/method-dto.model";

@Component({
    selector: 'app-create-method',
    templateUrl: './create-method.component.html',
    styles: []
})

export class CreateMethodComponent {
    createMethodForm: FormGroup;
    status: { loading?: true; error?: string } | null = null;
    
    constructor(
        private readonly _router: Router,
        @Inject(MethodService) private readonly _methodService: MethodService
    ) {
        this.createMethodForm = new FormGroup({
            name: new FormControl("", [Validators.required]),
            type: new FormControl("", [Validators.required]),
        });
    }

    protected atSend(method: MethodCreateModel) {
        this.status = { loading: true };

        this._methodService.createMethod(method).subscribe({
            next: (response) => {
                this.status = null;
                this._router.navigate([""]);
            },
            error: (error:any) => {
                if (error.status === 400 && error.Message) {
                    this.status = { error: error.Message };
                } else {
                    this.status = { error: error.message || 'Error creating method.' };
                }
            },
        });
    }
}