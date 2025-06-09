import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MethodApiRepositoryService } from '../../repositories/method-api-repository.service';
import MethodDTO from './models/method-dto.model';
import { MethodListItem } from './models/method-list-item.model';

@Injectable({
  providedIn: 'root'
})
export class MethodService {

  constructor(private readonly _methodRepository: MethodApiRepositoryService) {}

  public createMethod(method: MethodDTO): Observable<MethodDTO> {
    return this._methodRepository.createMethod(method);
  }

  public deleteMethod(id: string): Observable<any> {
    return this._methodRepository.deleteMethod(id);
  }

  public getAllMethods(): Observable<MethodListItem[]> {
    return this._methodRepository.getAllMethods();
  }

  public getMethodById(methodId: string): Observable<any> {
    var answer = this.getMethodById(methodId);
    return answer;
  }
  
  public addInvokeMethods(){
    //TODO
  }
}