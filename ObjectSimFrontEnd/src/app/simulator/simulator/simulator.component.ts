import { Component, Inject } from '@angular/core';
import { Router } from "@angular/router";
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { SimulatorService } from "../../../backend/services/simulator/simulator.service";
import CreateSimulatedExecutionModel from "../../../backend/services/simulator/models/create-simulated-execution.model"

@Component({
    selector: 'app-simulator',
    templateUrl: './simulator.component.html'
})

export class SimulatorComponent {

    status: { loading?: true; error?: string } | null = null;

    constructor(
        private readonly _router: Router,
        @Inject(SimulatorService) private readonly _simulatorService : SimulatorService
    ) {
        console.log('SimulatorComponent inicializado');
    }

}