import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import CreateAttributeModel from '../../backend/services/attribute/models/create-attribute.model';

@Injectable({
    providedIn: 'root',
})
export class AttributeApiRepository extends ApiRepository {

    constructor(http: HttpClient) {
        super(enviroments.objectsim, 'api/attributes', http);
    }

    createAttribute(attribute: CreateAttributeModel): Observable<CreateAttributeModel> {
        return this.post<CreateAttributeModel>(attribute);
    }

    getAllAttributes(): Observable<any>{
        //TODO
        return this.get<any>();
    }

    deleteAttribute(id: string): Observable<any> {
        const url = `${id}`;
        return this.delete(url);
    }

    getAttributeById(attributeId: string): Observable<CreateAttributeModel> {
        return this.get<CreateAttributeModel>(attributeId);
    }

    getAttributesByClassId(classId: string): Observable<CreateAttributeModel[]> {
        return this.get<CreateAttributeModel[]>(`by-class/${classId}`);
    }

    updateAttribute(id: string, attribute: CreateAttributeModel): Observable<CreateAttributeModel> {
        return this.patch<CreateAttributeModel>(`${id}`, attribute);
    }
}