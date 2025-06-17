import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { CreateClassComponent } from './create-class/create-class.component';
import { DeleteClassComponent } from './delete-class/delete-class.component';
import { ClassRoutingModule } from './class-routing.module';
import { DeleteClassFormComponent } from '../../business-components/class/delete-class/delete-class-form.component';
import { CreateClassFormComponent } from '../../business-components/create-class-form/create-class-form.component';
import { ButtonComponent } from '../../components/button/button.component';
import {ClassesListingComponent} from "./class-list/classes-listing.component";
import { ClassListComponent } from "../../business-components/class/class-list/class-list.component";
import {ClassInfoComponent} from "./class-info/class-info.component";
import {ShowClassInfoComponent} from "../../business-components/class/class-info/show-class-info.component";

@NgModule({
    declarations: [
        CreateClassComponent,
        DeleteClassComponent,
        ClassesListingComponent,
        ClassInfoComponent
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        ClassRoutingModule,
        CreateClassFormComponent,
        ButtonComponent,
        DeleteClassFormComponent,
        ClassListComponent,
        ShowClassInfoComponent
    ]
})
export class ClassModule { }
