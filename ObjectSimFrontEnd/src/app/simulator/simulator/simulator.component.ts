import { Component, Inject } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

import { SimulatorService } from "../../../backend/services/simulator/simulator.service";
import CreateSimulatedExecutionModel from "../../../backend/services/simulator/models/create-simulated-execution.model"

@Component({
    selector: 'app-simulator',
    templateUrl: './simulator.component.html'
})

export class SimulatorComponent {

    status: { loading?: true; error?: string } | null = null;
    simulationResult: SafeHtml | string = '';

    constructor(
        @Inject(SimulatorService) private readonly _simulatorService : SimulatorService,
        private sanitizer: DomSanitizer
    ) {
    }

    protected atSubmit(simulatedExecution: CreateSimulatedExecutionModel) {
        this.status = { loading: true };

        this._simulatorService.simulateExecution(simulatedExecution).subscribe({
            next: (response: { format: string; content: string }) => {
                console.log('Response from backend:', response);
                this.status = null;
                this.simulationResult = this.sanitizer.bypassSecurityTrustHtml(response.content);
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


    copyResult() {
        const result = this.sanitizer.sanitize(1, this.simulationResult) || '';
        navigator.clipboard.writeText(result);
    }
}