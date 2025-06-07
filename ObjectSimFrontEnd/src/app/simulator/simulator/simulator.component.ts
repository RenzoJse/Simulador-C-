import { Component, Inject } from '@angular/core';
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
        @Inject(SimulatorService) private readonly _simulatorService : SimulatorService
    ) {
        console.log('SimulatorComponent inicializado');
    }

    protected atSubmit(simulatedExecution: CreateSimulatedExecutionModel) {
        this.status = { loading: true };

        this._simulatorService.simulateExecution(simulatedExecution).subscribe({
            next: (response) => {
                this.status = null;
            },
            error: (error:any) => {
                if (error.status === 400 && error.Message) {
                    this.status = { error: error.Message };
                } else {
                    this.status = { error: error.message || 'Error in simulation.' };
                }
            },
        });
    }
}