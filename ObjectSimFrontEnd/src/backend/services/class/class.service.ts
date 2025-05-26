import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import CreateClassModel from './models/create-class.model';

@Injectable({
    providedIn: 'root'
})
export class ClassService {
    private readonly apiUrl = 'http://localhost:5018/class';
    
    constructor(private http: HttpClient) {}
    
    createClass(classObj: CreateClassModel): Observable<any> {
        return this.http.post(this.apiUrl, classObj);
    }
}