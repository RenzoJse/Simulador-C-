import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";
import { SesionRoutingModule } from './sesion-routing.module';
import { InsertKeyFormComponent } from '../../business-components/sesion/insert-key-form.component';
import { KeyComponent } from './insertKey/key.component'

@NgModule({
    declarations: [
        KeyComponent
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        SesionRoutingModule,
        InsertKeyFormComponent
    ]
})
export class SesionModule { }