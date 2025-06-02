import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { SimulatorRoutingModule } from './simulator-routing.module';

@NgModule({
    declarations: [
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        SimulatorRoutingModule,
    ]
})
export class SimulatorModule { }