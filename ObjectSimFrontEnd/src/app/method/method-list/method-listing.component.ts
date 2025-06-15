import { Component } from '@angular/core';
import MethodDTO from "../../../backend/services/method/models/method-dto.model";
import {Router} from "@angular/router";
@Component({
    selector: 'app-method-listing',
    templateUrl: './method-listing.component.html',
})

export class MethodListingComponent {

    selectedMethod: string | null = null;

    constructor(private router: Router) {}

    onSelectedMethod(methodId: string | undefined): void {
        if (methodId) {
            this.selectedMethod = methodId;
            this.router.navigate([methodId, 'invoke-method']);
        } else {
            this.selectedMethod = null;
        }
    }
}


