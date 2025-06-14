import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { SimulatorFormComponent } from '../../business-components/simulator/simulator-form.component';
import { ButtonComponent } from '../../components/button/button.component';

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
        SimulatorFormComponent,
        ButtonComponent
    ]
})
export class SimulatorModule { }