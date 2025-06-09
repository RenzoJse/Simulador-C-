import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ClassApiRepository } from '../../repositories/class-api-repository.service';
import CreateClassModel from './models/create-class.model';
import ClassListItem from './models/class-list-item';

@Injectable({
    providedIn: 'root'
})
export class ClassService {
    
    constructor(private readonly  _classRepository: ClassApiRepository) {}

    public createClass(classObj: CreateClassModel): Observable<CreateClassModel> {
      return this._classRepository.createClass(classObj);
    }

    getAllClasses(): Observable<ClassListItem[]> {
        return this._classRepository.getAllClasses();
    }

    deleteClass(id: string): Observable<any> {
        return this._classRepository.deleteClass(id);
    }
}