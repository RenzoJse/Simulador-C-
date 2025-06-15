import { Component, Inject } from '@angular/core';

import { MethodService } from "../../../backend/services/method/method.service";
import AddInvokeMethodModel from "../../../backend/services/method/models/add-invoke-method.model";
import { ActivatedRoute } from "@angular/router";

@Component({
    selector: 'app-add-invoke-method',
    templateUrl: './add-invoke-method.component.html',
})

export class AddInvokeMethodComponent {

    status: { loading?: true; error?: string } | null = null;
    private methodId!: string;

    constructor(
        @Inject(MethodService) private readonly _methodService: MethodService,
        private route: ActivatedRoute
    ) {
        this.route.paramMap.subscribe(params => {
            this.methodId = params.get('methodId')!;
        });
    }

    protected atSubmit(methodToAdd: AddInvokeMethodModel) {
        this.status = { loading: true };

        this._methodService.addInvokeMethods(this.methodId, [methodToAdd]).subscribe({
            next: () => {
                this.status = null;
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