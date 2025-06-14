import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateMethodComponent } from './create-method/create-method.component';
import { DeleteMethodComponent } from './delete-method/delete-method.component';
import { MethodListingComponent } from './method-list/method-listing.component';

const routes: Routes = [
    { path: 'create', component: CreateMethodComponent },
    { path: 'delete', component: DeleteMethodComponent },
    { path: 'list', component: MethodListingComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class MethodRoutingModule { }
