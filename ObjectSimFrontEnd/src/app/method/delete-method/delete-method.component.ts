import { Component, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MethodService } from '../../../backend/services/method/method.service';

@Component({
  selector: 'app-delete-method',
  templateUrl: './delete-method.component.html'
})

export class DeleteMethodComponent {

  status: { loading?: true; error?: string } | null = null;

  constructor(
      @Inject(MethodService) private readonly _methodService : MethodService
  ) {
  }

  protected atSubmit(methodId: string) {
    this.status = { loading: true };
    this._methodService.deleteMethod(methodId).subscribe({
      next: (response) => {
        this.status = null;
      },
      error: (error:any) => {
        if (error.status === 400 && error.Message) {
          this.status = { error: error.Message };
        } else {
          this.status = { error: error.message || 'Error in deleting method.' };
        }
      },
    });
  }

}