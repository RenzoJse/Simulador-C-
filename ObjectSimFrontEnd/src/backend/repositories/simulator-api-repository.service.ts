import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import CreateSimulatedExecutionModel from '../../backend/services/simulator/models/create-simulated-execution.model';

@Injectable({
    providedIn: 'root',
})
export class SimulatorApiRepository extends ApiRepository {
    
    constructor(http: HttpClient) {
        super(enviroments.objectsim, 'api/simulator', http);
    }

    simulateExecution(simulationModel: CreateSimulatedExecutionModel): Observable<CreateSimulatedExecutionModel> {
        const customHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            Authorization: localStorage.getItem('key') ?? '',
        });
        return this.post<CreateSimulatedExecutionModel>(simulationModel, '', customHeaders);
    }
}
    
        