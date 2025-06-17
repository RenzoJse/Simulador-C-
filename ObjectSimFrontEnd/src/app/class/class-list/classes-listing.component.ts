import { Component } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import ClassDtoOut from "../../../backend/services/class/models/class-dto-out";

@Component({
    selector: 'app-classes-listed',
    templateUrl: './classes-listing.component.html',
    styles: []
})

export class ClassesListingComponent{

    selectedClass: string | null = null;
    showConfirmationPopup: boolean = false;

    constructor(private router: Router, private route: ActivatedRoute) {
    }

    onSelectedClass(classId: string | undefined): void {
        if (classId) {
            this.selectedClass = classId;
            this.showConfirmationPopup = true;
        } else {
            this.selectedClass = null;
            this.showConfirmationPopup = false;
        }
    }

    confirmNavigation(method: string): void {
        console.log(`Confirmed to edit class with method: ${method}`);
        this.router.navigate([method], { relativeTo: this.route.parent });
        this.closePopup();
    }

    closePopup(): void {
        this.showConfirmationPopup = false;
    }
}


