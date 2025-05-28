import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { CreateMethodComponent } from './create-method/create-method.component';
import { MethodRoutingModule } from './method-routing.module';

import { CreateMethodFormComponent } from '../../business-components/method/create-method-form/create-method-form.component'; 
import { ButtonComponent } from '../../components/button/button.component';

import { DeleteMethodComponent } from './delete-method/delete-method.component';
import { DeleteMethodFormComponent } from '../../business-components/method/delete-method/delete-method-form.component';

@NgModule({
  declarations: [
    CreateMethodComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MethodRoutingModule,
    CreateMethodFormComponent,
    DeleteMethodFormComponent,
    ButtonComponent,
    DeleteMethodComponent,
  ]
})
export class MethodModule { }