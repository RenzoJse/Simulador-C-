import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { ButtonComponent } from '../../components/button/button.component';

import { UploadOutputModelRoutingModule } from './upload-outputModel-routing.module';
import { UploadOutputModelComponent } from "./upload-outputModel/upload-outputModel.component";
import {
    UploadOutputModelFormComponent
} from "../../business-components/outputModel/upload-outputModel-form/upload-outputModel-form.component";

@NgModule({
    declarations: [
        UploadOutputModelComponent
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        UploadOutputModelRoutingModule,
        ButtonComponent,
        UploadOutputModelFormComponent,
    ]
})
export class UploadOutputModelModule { }