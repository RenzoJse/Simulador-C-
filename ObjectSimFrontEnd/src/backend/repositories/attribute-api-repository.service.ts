import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import AttributeDtoModel from '../../backend/services/attribute/model/attribute-dto.model';

@Injectable({
    providedIn: 'root',
})
export class AttributeApiRepository extends ApiRepository {
    
    constructor(http: HttpClient) {
        super(enviroments.objectsim, 'api/attributes', http);
    }
    
    createAttribute(attribute: AttributeDtoModel): Observable<AttributeDtoModel> {
        return this.post<AttributeDtoModel>(attribute);
    }
    
    getAllAttributes(): Observable<any>{
        //TODO
        return this.get<any>();
    }
    
    deleteAttribute(id: string): Observable<any> {
        const url = `/${id}`;
        return this.delete(url);
    }
    
    getAttributeById(attributeId: string): Observable<AttributeDtoModel> {
        return this.get<AttributeDtoModel>(attributeId);
    }
    
    getAttributesByClassId(classId: string): Observable<AttributeDtoModel[]> {
        return this.get<AttributeDtoModel[]>(`by-class/${classId}`);
    }
    
    updateAttribute(id: string, attribute: AttributeDtoModel): Observable<AttributeDtoModel> {
        return this.patch<AttributeDtoModel>(`${id}`, attribute);
    }
}