import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateNamespaceComponent } from './create-namespace/create-namespace.component';

const routes: Routes = [
  { path: 'create', component: CreateNamespaceComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NamespaceRoutingModule {}
