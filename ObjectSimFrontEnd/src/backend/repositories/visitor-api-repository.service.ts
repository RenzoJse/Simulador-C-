import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import environments from '../../environments/index';
import ApiRepository from './api-repository';

@Injectable({
  providedIn: 'root',
})
export class VisitorApiRepository extends ApiRepository {

  constructor(http: HttpClient) {
    super(environments.objectsim, 'api/examples', http);
  }

  loadExample(): Observable<void> {
    return this.post<void>({});
  }
}
