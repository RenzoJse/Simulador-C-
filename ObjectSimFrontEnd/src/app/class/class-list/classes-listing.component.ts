import { Component } from '@angular/core';
import CreateClassModel from "../../../backend/services/class/models/create-class.model";

@Component({
    selector: 'app-classes-listed',
    templateUrl: './classes-listing.component.html',
    styles: []
})

export class ClassesListingComponent{

    selectedClass: string | null = null;
    showConfirmationPopup: boolean = false;

    onSelectedClass(classObj: CreateClassModel | undefined): void {
        if (classObj) {
            this.selectedClass = classObj.name;
            this.showConfirmationPopup = true;
        } else {
            this.selectedClass = null;
            this.showConfirmationPopup = false;
        }
    }

    confirmNavigation(method: string): void {
        console.log(`Confirmed to edit class with method: ${method}`);
        this.showConfirmationPopup = false;
    }

    closePopup(): void {
        this.showConfirmationPopup = false;
    }
}


