import { Component } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";

@Component({
    selector: 'app-method-listing',
    templateUrl: './method-listing.component.html'
})

export class MethodListingComponent {

    selectedMethod: string | null = null;
    showConfirmationPopup: boolean = false;
    constructor(private router: Router, private route: ActivatedRoute) {}

    onSelectedMethod(methodId: string | undefined): void {
        if (methodId) {
            this.selectedMethod = methodId;
            this.showConfirmationPopup = true;
        } else {
            this.selectedMethod = null;
        }
    }

    confirmNavigation(methodId: string | null): void {
        if (this.selectedMethod) {
            this.router.navigate([methodId, 'invoke-method'], { relativeTo: this.route.parent });
        }
        this.closePopup();
    }

    closePopup(): void {
        this.selectedMethod = null;
        this.showConfirmationPopup = false;
    }
}


