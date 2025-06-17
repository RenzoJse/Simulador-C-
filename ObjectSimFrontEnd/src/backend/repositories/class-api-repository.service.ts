import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import CreateClassModel from '../../backend/services/class/models/create-class.model';
import ClassListItem from '../../backend/services/class/models/class-list-item';
import UpdateClassModel from '../services/class/models/update-class-model';
import ClassDtoOut from "../services/class/models/class-dto-out";

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

    getById(classId: string): Observable<ClassDtoOut[]> {
        return this.get<ClassDtoOut[]>(classId);
    }

    deleteClass(id: string): Observable<any> {
        const url = `${id}`;
        return this.delete(url);
    }

    updateClass(id: string, updateModel: UpdateClassModel): Observable<any> {
        const url = `${id}`;
        return this.patch(url, updateModel);
    }
}

