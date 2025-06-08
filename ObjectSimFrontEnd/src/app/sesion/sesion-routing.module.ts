import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { KeyComponent } from './insertKey/key.component';

const routes: Routes = [
    { path: 'key', component: KeyComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class SesionRoutingModule { }
