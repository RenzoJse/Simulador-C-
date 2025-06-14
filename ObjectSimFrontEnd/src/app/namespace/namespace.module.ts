import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NamespaceRoutingModule } from './namespace-routing.module';
import { CreateNamespaceComponent } from './create-namespace/create-namespace.component';

@NgModule({
  imports: [
    CommonModule,
    NamespaceRoutingModule,
  ]
})
export class NamespaceModule {}
