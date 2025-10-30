import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UploadOutputModelComponent } from "./upload-outputModel/upload-outputModel.component";

const routes: Routes = [
    { path: 'upload', component: UploadOutputModelComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class UploadOutputModelRoutingModule { }
