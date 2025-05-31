import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateAttributeComponent } from './create-attribute/create-attribute.component';
import { DeleteAttributeComponent } from './delete-attribute/delete-attribute.component';

const routes: Routes = [
  { path: 'create', component: CreateAttributeComponent },
  { path: 'delete', component: DeleteAttributeComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttributeRoutingModule {}