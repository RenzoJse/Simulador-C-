import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SimulatorApiRepository } from '../../repositories/simulator-api-repository.service';
import CreateSimulatedExecutionModel from './models/create-simulated-execution.model';

@Injectable({
    providedIn: 'root'
})
export class SimulatorService {
    
    constructor(private readonly _simulatorRepository: SimulatorApiRepository) {}

    public simulateExecution(simulateExecution: CreateSimulatedExecutionModel): Observable<any> {
        return this._simulatorRepository.simulateExecution(simulateExecution);
    }

}