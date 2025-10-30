import { Injectable } from '@angular/core';
import { NamespaceApiRepository } from '../../repositories/namespace-api-repository.service';
import { Observable } from 'rxjs';
import CreateNamespaceModel from './model/create-namespace.model';
import NamespaceDto from './model/namespace-dto.model';

@Injectable({
  providedIn: 'root'
})
export class NamespaceService {
  constructor(private readonly _namespaceRepository: NamespaceApiRepository) {}

  public createNamespace(namespace: CreateNamespaceModel): Observable<CreateNamespaceModel> {
    return this._namespaceRepository.createNamespace(namespace)
  }

  public getAllNamespaces(): Observable<NamespaceDto[]> {
  return this._namespaceRepository.getAllNamespaces();
  }

  getDescendants(namespaceId: string): Observable<NamespaceDto[]> {
  return this._namespaceRepository.getDescendants(namespaceId);
  }


}
