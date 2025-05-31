import { Component, Inject } from '@angular/core';
import { Router } from "@angular/router";
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { AttributeService } from "../../../backend/services/attribute/attribute.service";
import AttributeDto from '../../../backend/services/attribute/model/attribute-dto.model';

@Component({
  selector: 'app-create-attribute',
  templateUrl: './create-attribute.component.html',
  styles: []
})
export class CreateAttributeComponent {
 createAttributeForm: FormGroup;
  status: { loading?: true; error?: string } | null = null;

  constructor(
    private readonly _router: Router,
    @Inject(AttributeService) private readonly _attributeService: AttributeService
  ) {
    this.createAttributeForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      dataTypeName: new FormControl('', [Validators.required]),
      dataTypeKind: new FormControl('', [Validators.required]),
      visibility: new FormControl('', [Validators.required]),
      classId: new FormControl('', [Validators.required])
    });
  }

  protected atSubmit(attribute: AttributeDto) {
    this.status = { loading: true };
    console.log("Enviando atributo:", attribute);

    this._attributeService.createAttribute(attribute).subscribe({
      next: () => {
        this.status = null;
        this._router.navigate(['']);
      },
      error: (error: any) => {
        if (error.status === 400 && error.Message) {
          this.status = { error: error.Message };
        } else {
          this.status = { error: error.message || 'Error creating attribute.' };
        }
      },
    });
  }
}
