import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { SimulatorFormComponent } from '../../business-components/simulator/simulator-form.component';

import { SimulatorRoutingModule } from './simulator-routing.module';
import { SimulatorComponent } from './simulator/simulator.component';

@NgModule({
    declarations: [
        SimulatorComponent
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        SimulatorRoutingModule,
        SimulatorFormComponent
    ]
})
export class SimulatorModule { }