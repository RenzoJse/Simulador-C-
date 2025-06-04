import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateNamespaceComponent } from './create-namespace/create-namespace.component';
import { GetAllNamespacesComponent } from './getall-namespace/get-all-namespace.component';

const routes: Routes = [
  { path: 'create', component: CreateNamespaceComponent },
  { path: 'list', component: GetAllNamespacesComponent }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NamespaceRoutingModule {}
