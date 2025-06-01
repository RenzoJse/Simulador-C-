import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import AttributeDto from './model/attribute-dto.model';
import CreateAttributeModel from '../class/models/create-attribute.model';

@Injectable({
  providedIn: 'root'
})
export class AttributeService {
  private readonly apiUrl = 'http://localhost:5018/api/attributes';

  constructor(private http: HttpClient) {}

  createAttribute(dto: CreateAttributeModel): Observable<AttributeDto> {
    return this.http.post<AttributeDto>(this.apiUrl, dto);
  }

  getAttributes(): Observable<AttributeDto[]> {
    return this.http.get<AttributeDto[]>(this.apiUrl);
  }

  getAttributeById(id: string): Observable<AttributeDto> {
    return this.http.get<AttributeDto>(`${this.apiUrl}/${id}`);
  }

  getAttributesByClassId(classId: string): Observable<AttributeDto[]> {
    return this.http.get<AttributeDto[]>(`${this.apiUrl}/by-class/${classId}`);
  }

updateAttribute(id: string, dto: CreateAttributeModel): Observable<AttributeDto> {
  return this.http.put<AttributeDto>(`${this.apiUrl}/${id}`, dto);
}

  deleteAttribute(id: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${id}`);
  }
}