import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { MethodExecutionRoutingModule } from './method-execution-routing.module';

@NgModule({
    declarations: [
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MethodExecutionRoutingModule,
    ]
})
export class MethodExecutionModule { }