import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AttributeApiRepository } from '../../repositories/attribute-api-repository.service';
import CreateAttributeModel from './models/create-attribute.model';
import AttributeUpdateModel from './models/attribute-update.model';

@Injectable({
  providedIn: 'root'
})
export class AttributeService {

  constructor(private readonly _attributeRepository: AttributeApiRepository) {}

  public createAttribute(attribute: CreateAttributeModel): Observable<CreateAttributeModel> {
    return this._attributeRepository.createAttribute(attribute);
  }

  public getAllAttributes(): Observable<AttributeUpdateModel[]> {
    return this._attributeRepository.getAllAttributes();
  }

  public deleteAttribute(id: string): Observable<boolean> {
    return this._attributeRepository.deleteAttribute(id);
  }

  public getAttributeById(id: string): Observable<AttributeUpdateModel> {
    return this._attributeRepository.getAttributeById(id);
  }

  public getAttributesByClassId(classId: string): Observable<AttributeUpdateModel[]> {
    return this._attributeRepository.getAttributesByClassId(classId);
  }

  public updateAttribute(id: string, dto: AttributeUpdateModel): Observable<AttributeUpdateModel> {
    return this._attributeRepository.updateAttribute(id, dto);
  }

}