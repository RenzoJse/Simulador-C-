import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import CreateSimulatedExecutionModel from './models/create-simulated-execution.model';

@Injectable({
    providedIn: 'root'
})
export class SimulatorService {
    private readonly apiUrl = 'http://localhost:5018/api/simulator';

    constructor(private http: HttpClient) {}

    simulateExecution(simulateExecution: CreateSimulatedExecutionModel): Observable<any> {
        return this.http.post(this.apiUrl, simulateExecution);
    }

}