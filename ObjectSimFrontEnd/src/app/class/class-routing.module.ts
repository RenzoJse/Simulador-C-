import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateClassComponent } from './create-class/create-class.component';
import { DeleteClassComponent } from './delete-class/delete-class.component';
import { UpdateClassComponent } from './update-class/update-class.component';
import { ClassesListingComponent } from './class-list/classes-listing.component';

const routes: Routes = [
    { path: 'create', component: CreateClassComponent },
    { path: 'delete', component: DeleteClassComponent },
    { path: 'update', component: UpdateClassComponent },
    { path: 'list', component: ClassesListingComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ClassRoutingModule { }