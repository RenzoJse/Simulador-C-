import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'inicio',
    loadChildren: () => import('./landing-page/landing-page.module').then(m => m.LandingPageModule),
  },
  {
    path: 'method',
    loadChildren: () => import('./method/method.module').then(m => m.MethodModule),
  }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
