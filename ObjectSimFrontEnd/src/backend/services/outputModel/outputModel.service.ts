import { Injectable } from '@angular/core';
import { OutputModelApiRepositoryService } from '../../repositories/outputModel-api-repository.service';
import { Observable } from 'rxjs';
import OutputModelName from "./model/outputModelName";

@Injectable({
    providedIn: 'root'
})
export class OutputModelService {
    constructor(private readonly _outputModelRepository: OutputModelApiRepositoryService) {}

    public uploadDllFile(file: File): Observable<any> {
        return this._outputModelRepository.uploadDllFile(file);
    }

    public getImplementationList(): Observable<OutputModelName[]> {
        return this._outputModelRepository.getImplementationList();
    }

}
