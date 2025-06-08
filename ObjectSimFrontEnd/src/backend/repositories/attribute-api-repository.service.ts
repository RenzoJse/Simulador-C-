import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import MethodDtoModel from '../../backend/services/method/models/method-dto.model';
import { MethodListItem } from '../../backend/services/method/models/method-list-item.model';

@Injectable({
    providedIn: 'root',
})
export class AttributeApiRepositoryService extends ApiRepository {

    constructor(http: HttpClient) {
        super(enviroments.objectsim, 'api/methods', http);
    }

    createMethod(method: MethodDtoModel): Observable<MethodDtoModel> {
        return this.post<MethodDtoModel>(method);
    }

    getAllMethods(): Observable<MethodListItem[]> {
        return this.get<MethodListItem[]>();
    }

    deleteMethod(id: string): Observable<any> {
        const url = `/${id}`;
        return this.delete(url);
    }
    
    getMethodById(methodId: string): Observable<MethodDtoModel> {
        var answer = this.get<MethodDtoModel>(methodId);
        return answer;
    }
    
    addInvokeMethods(){
        //TODO
    }
    
}
    
        