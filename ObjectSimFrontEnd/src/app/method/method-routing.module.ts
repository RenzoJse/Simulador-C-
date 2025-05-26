import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateMethodComponent } from './create-method/create-method.component';

const routes: Routes = [
    { path: 'create', component: CreateMethodComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class MethodRoutingModule { }
