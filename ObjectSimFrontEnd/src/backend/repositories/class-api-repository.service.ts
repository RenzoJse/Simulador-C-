import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import CreateClassModel from '../../backend/services/class/models/create-class.model';
import ClassListItem from '../../backend/services/class/models/class-list-item';

@Injectable({
    providedIn: 'root',
})
export class ClassApiRepository extends ApiRepository {

    constructor(http: HttpClient) {
        super(enviroments.objectsim, 'api/classes', http);
    }

    createClass(classObj: CreateClassModel): Observable<CreateClassModel> {
        return this.post<CreateClassModel>(classObj);
    }

    getAllClasses(): Observable<ClassListItem[]> {
        return this.get<ClassListItem[]>();
    }

    getById(id: string): Observable<ClassListItem[]> {
        const url = `${id}`;
        return this.get<ClassListItem[]>(); //está mal, reutilice el getAll hay que ver como implementar el getbyid
    }

    deleteClass(id: string): Observable<any> {
        const url = `${id}`;
        return this.delete(url);
    }

    updateClass(id: string): Observable<any> {
        const url = `${id}`;
        return this.delete(url); //cambiar
    }
}

