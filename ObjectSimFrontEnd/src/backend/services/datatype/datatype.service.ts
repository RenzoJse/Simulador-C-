import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataTypeApiRepository } from '../../repositories/datatype-api-repository.service';
import DataTypeModel from './models/datatype.model';
@Injectable({
  providedIn: 'root'
})
export class DataTypeService {
  constructor(private readonly _dataTypeRepo: DataTypeApiRepository) {}

  getAll(): Observable<DataTypeModel[]> {
    return this._dataTypeRepo.getAll();
  }
}
