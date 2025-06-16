import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { VisitorApiRepository } from '../../../repositories/visitor-api-repository.service';

@Injectable({ providedIn: 'root' })
export class VisitorService {
  constructor(private readonly _visitorRepository: VisitorApiRepository) {}

  public LoadExample() {
      return this.LoadExample();
    }
}
