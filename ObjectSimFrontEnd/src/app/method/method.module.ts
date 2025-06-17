import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from "@angular/forms";

import { CreateMethodComponent } from './create-method/create-method.component';
import { MethodListingComponent } from "./method-list/method-listing.component";
import { DeleteMethodComponent } from './delete-method/delete-method.component';
import { MethodRoutingModule } from './method-routing.module';

import { CreateMethodFormComponent } from '../../business-components/method/create-method-form/create-method-form.component';
import { ButtonComponent } from '../../components/button/button.component';
import { MethodDropdownComponent } from '../../business-components/method/dropdown/method-dropdown.component';
import { MethodListComponent } from "../../business-components/method/method-list/method-list.component";
import { DeleteMethodFormComponent } from '../../business-components/method/delete-method/delete-method-form.component';
import { AddInvokeMethodComponent } from "./add-invoke-method/add-invoke-method.component";
import { InvokeMethodFormComponent } from "../../business-components/method/invoke-method-form/invoke-method-form.component";
import { MethodInfoComponent } from "./method-info/method-info.component";
import { ShowMethodInfoComponent } from "../../business-components/method/method-info/show-method-info.component";
import {ConfirmationPopupComponent} from "../../components/confirmation-popup/confirmation-popup.component";

@NgModule({
  declarations: [
    CreateMethodComponent,
    DeleteMethodComponent,
    MethodListingComponent,
    AddInvokeMethodComponent,
    MethodInfoComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MethodRoutingModule,
    CreateMethodFormComponent,
    MethodDropdownComponent,
    DeleteMethodFormComponent,
    ButtonComponent,
    MethodListComponent,
    InvokeMethodFormComponent,
    ShowMethodInfoComponent,
    ConfirmationPopupComponent
  ]
})
export class MethodModule { }