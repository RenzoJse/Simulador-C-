import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments';
import ApiRepository from './api-repository';
import DataTypeModel from '../services/datatype/models/datatype.model';

@Injectable({
  providedIn: 'root'
})
export class DataTypeApiRepository extends ApiRepository {

  constructor(http: HttpClient) {
    super(enviroments.objectsim, 'api/datatypes', http);
  }

  getAll(): Observable<DataTypeModel[]> {
    return this.get<DataTypeModel[]>();
  }
}
