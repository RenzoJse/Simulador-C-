import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateMethodComponent } from './method/create-method/create-method.component'; 

const routes: Routes = [
  {
    path: 'inicio',
    loadChildren: () => import('./landing-page/landing-page.module').then(m => m.LandingPageModule),
  },
  {
    path: 'methods/create',
    component: CreateMethodComponent
  }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
