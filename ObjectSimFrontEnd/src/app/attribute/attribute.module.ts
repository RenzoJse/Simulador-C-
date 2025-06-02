import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { AttributeRoutingModule } from './attribute-routing.module';

import { CreateAttributeComponent } from './create-attribute/create-attribute.component';
import { CreateAttributeFormComponent } from '../../business-components/attribute/create-attribute-form/create-attribute-form.component';
import { FormsModule } from '@angular/forms';
import { ButtonComponent } from '../../components/button/button.component';
import { DeleteAttributeComponent } from './delete-attribute/delete-attribute.component';

@NgModule({
  declarations: [
    CreateAttributeComponent,
    DeleteAttributeComponent
    ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AttributeRoutingModule,
    CreateAttributeFormComponent,
    ButtonComponent,
    FormsModule
  ]
})
export class AttributeModule {}
