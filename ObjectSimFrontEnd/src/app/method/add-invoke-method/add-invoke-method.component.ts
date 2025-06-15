import { Component, Inject } from '@angular/core';

import { MethodService } from "../../../backend/services/method/method.service";
import AddInvokeMethodModel from "../../../backend/services/method/models/add-invoke-method.model";

@Component({
    selector: 'app-add-invoke-method',
    templateUrl: './add-invoke-method.component.html',
})

export class AddInvokeMethodComponent {

    status: { loading?: true; error?: string } | null = null;

    constructor(
        @Inject(MethodService) private readonly _methodService : MethodService,
    ) {
    }

    protected atSubmit(methodToAdd: AddInvokeMethodModel) {
        this.status = { loading: true };

        this._methodService.addInvokeMethods(methodToAdd).subscribe({
            next: () => {


            },
            error: (error:any) => {
                if (error.status === 400 && error.Message) {
                    this.status = { error: error.Message };
                } else {
                    this.status = { error: error.message || 'Error adding invoke method.' };
                }
            },
        });
    }

}