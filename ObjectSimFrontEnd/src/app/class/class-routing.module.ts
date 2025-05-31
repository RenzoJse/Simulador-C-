import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateClassComponent } from './create-class/create-class.component';

const routes: Routes = [
    { path: 'create', component: CreateClassComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ClassRoutingModule { }