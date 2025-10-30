import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import CreateSimulatedExecutionModel from '../../backend/services/simulator/models/create-simulated-execution.model';

@Injectable({
    providedIn: 'root',
})
export class OutputModelApiRepositoryService extends ApiRepository {

    constructor(http: HttpClient) {
        super(enviroments.objectsim, 'api/outputModel', http);
    }

    uploadDllFile(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('dllFile', file);
        return this.post<any>(formData);
    }

    getImplementationList(): Observable<any> {
        return this.get<any>();
    }

}

