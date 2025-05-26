import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import MethodDTO from './models/method-dto.model';

@Injectable({
  providedIn: 'root'
})
export class MethodService {
  private readonly apiUrl = 'http://localhost:5018/api/methods';

  constructor(private http: HttpClient) {}

  createMethod(method: MethodDTO): Observable<any> {
    return this.http.post(this.apiUrl, method);
  }

  deleteMethod(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getMethods(): Observable<MethodDTO[]> {
    return this.http.get<MethodDTO[]>(this.apiUrl);
  }
}