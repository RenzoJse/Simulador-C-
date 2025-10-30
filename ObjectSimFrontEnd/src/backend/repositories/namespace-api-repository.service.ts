import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import enviroments from '../../environments/index';
import ApiRepository from './api-repository';
import CreateNamespaceModel from '../../backend/services/namespace/model/create-namespace.model';
import NamespaceDto from '../../backend/services/namespace/model/namespace-dto.model';


@Injectable({
    providedIn: 'root',
})
export class NamespaceApiRepository extends ApiRepository {

    constructor(http: HttpClient) {
        super(enviroments.objectsim, 'api/namespaces', http);
    }

    createNamespace(namespace: CreateNamespaceModel): Observable<CreateNamespaceModel> {
        return this.post<CreateNamespaceModel>(namespace);
    }

    getAllNamespaces(): Observable<NamespaceDto[]> {
        return this.get<NamespaceDto[]>();
    }
    getDescendants(namespaceId: string): Observable<NamespaceDto[]> {
  return this.get<NamespaceDto[]>(`${namespaceId}/descendants`);
}

}
  