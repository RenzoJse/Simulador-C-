import { Component } from '@angular/core';
import MethodDTO from "../../../backend/services/method/models/method-dto.model";
@Component({
    selector: 'app-method-listing',
    templateUrl: './method-listing.component.html',
})

export class MethodListingComponent {

    selectedMethod: string | null = null;

    onSelectedMethod(method: MethodDTO | undefined): void {
        if (method) {
            this.selectedMethod = method.name;
        } else {
            this.selectedMethod = null;
        }
    }
}


