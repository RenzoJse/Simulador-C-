import { Injectable } from '@angular/core';
import { OutputModelApiRepositoryService } from '../../repositories/outputModel-api-repository.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class OutputModelService {
    constructor(private readonly _outputModelRepository: OutputModelApiRepositoryService) {}

    public uploadDllFile(file: File): Observable<any> {
        return this._outputModelRepository.uploadDllFile(file);
    }

    public getImplementationList(): Observable<any> {
        return this._outputModelRepository.getImplementationList();
    }

}
