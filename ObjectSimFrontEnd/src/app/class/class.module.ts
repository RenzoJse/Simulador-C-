import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { CreateClassComponent } from './create-class/create-class.component';
import { DeleteClassComponent } from './delete-class/delete-class.component';
import { ClassRoutingModule } from './class-routing.module';
import { DeleteClassFormComponent } from '../../business-components/class/delete-class/delete-class-form.component';
import { CreateClassFormComponent } from '../../business-components/create-class-form/create-class-form.component';
import { ButtonComponent } from '../../components/button/button.component';

@NgModule({
    declarations: [
        CreateClassComponent,
        DeleteClassComponent
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        ClassRoutingModule,
        CreateClassFormComponent,
        ButtonComponent,
        DeleteClassFormComponent
    ]
})
export class ClassModule { }
