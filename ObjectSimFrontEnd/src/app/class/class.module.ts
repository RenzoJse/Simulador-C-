import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { CreateClassComponent } from './create-class/create-class.component';
import { ClassRoutingModule } from './class-routing.module';

import { CreateMethodFormComponent } from '../../business-components/create-method-form/create-method-form.component';
import { ButtonComponent } from '../../components/button/button.component';

@NgModule({
    declarations: [
        CreateClassComponent,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        ClassRoutingModule,
        CreateMethodFormComponent,
        ButtonComponent
    ]
})
export class MethodModule { }
