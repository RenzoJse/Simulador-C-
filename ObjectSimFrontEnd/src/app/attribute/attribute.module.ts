import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { AttributeRoutingModule } from './attribute-routing.module';

import { CreateAttributeComponent } from './create-attribute/create-attribute.component';
import { CreateAttributeFormComponent } from '../../business-components/create-attribute-form/create-attribute-form.component';

import { ButtonComponent } from '../../components/button/button.component';

@NgModule({
  declarations: [
    CreateAttributeComponent,
    ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AttributeRoutingModule,
    CreateAttributeFormComponent,
    ButtonComponent
  ]
})
export class AttributeModule {}
