import { Component } from '@angular/core';
import CreateClassModel from "../../../backend/services/class/models/create-class.model";


@Component({
    selector: 'app-classes-listed',
    templateUrl: './classes-listing.component.html',
    styles: []
})

export class ClassesListingComponent{

    selectedClass: string | null = null;

    onSelectedClass(classObj: CreateClassModel | undefined): void {
        if (classObj) {
            this.selectedClass = classObj.name;
        } else {
            this.selectedClass = null;
        }
    }
}


