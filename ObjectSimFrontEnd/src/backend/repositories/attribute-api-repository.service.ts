import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import CreateAttributeModel from '../../backend/services/attribute/models/create-attribute.model';
import AttributeUpdateModel from '../services/attribute/models/attribute-update.model';

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

    getAllAttributes(): Observable<AttributeUpdateModel[]>{
        return this.get<AttributeUpdateModel[]>();
    }

    deleteAttribute(id: string): Observable<any> {
        const url = `${id}`;
        return this.delete(url);
    }

    getAttributeById(attributeId: string): Observable<AttributeUpdateModel> {
        return this.get<AttributeUpdateModel>(attributeId);
    }

    getAttributesByClassId(classId: string): Observable<AttributeUpdateModel[]> {
        return this.get<AttributeUpdateModel[]>(`by-class/${classId}`);
    }

    updateAttribute(id: string, attribute: AttributeUpdateModel): Observable<AttributeUpdateModel> {
        return this.putById<AttributeUpdateModel>(id, attribute);
    }
}