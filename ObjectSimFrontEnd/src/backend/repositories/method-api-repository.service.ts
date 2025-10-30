import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import MethodDtoModel from '../../backend/services/method/models/method-dto.model';
import { MethodListItem } from '../services/method/models/method-list-item.model';
import AddInvokeMethodModel from "../services/method/models/add-invoke-method.model";

@Injectable({
    providedIn: 'root',
})
export class MethodApiRepositoryService extends ApiRepository {

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
        const url = `${id}`;
        return this.delete(url);
    }

    getMethodById(methodId: string): Observable<MethodDtoModel> {
        var answer = this.get<MethodDtoModel>(methodId);
        return answer;
    }

    addInvokeMethods(methodId: string, invokeMethods: AddInvokeMethodModel[]): Observable<MethodDtoModel> {
        const url = `${methodId}/invokeMethods`;
        console.log(JSON.stringify(invokeMethods));
        return this.patch<MethodDtoModel>(url, invokeMethods);
    }

}

