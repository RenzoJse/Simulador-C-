import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import CreateNamespaceModel from './model/create-namespace.model';

@Injectable({
  providedIn: 'root'
})
export class NamespaceService {
  private readonly apiUrl = 'http://localhost:5018/api/namespaces';

  constructor(private http: HttpClient) {}

  createNamespace(dto: CreateNamespaceModel): Observable<void> {
    return this.http.post<void>(this.apiUrl, dto);
  }

  getAllNamespaces(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
