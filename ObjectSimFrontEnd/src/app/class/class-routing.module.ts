import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateClassComponent } from './create-class/create-class.component';
import { DeleteClassComponent } from './delete-class/delete-class.component';

const routes: Routes = [
    { path: 'create', component: CreateClassComponent },
    { path: 'delete', component: DeleteClassComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ClassRoutingModule { }