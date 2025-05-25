import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { CreateMethodComponent } from './create-method/create-method.component';
import { MethodRoutingModule } from './method-routing.module';

import { CreateMethodFormComponent } from '../../business-components/create-method-form/create-method-form.component';

@NgModule({
    declarations: [
        CreateMethodComponent,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MethodRoutingModule,
        CreateMethodFormComponent
    ]
})
export class MethodModule { }
