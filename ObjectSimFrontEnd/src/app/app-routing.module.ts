import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authenticationGuard } from '../guard/authentication.guard';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'inicio'
  },
  {
  path: 'inicio',
  canActivate: [authenticationGuard],
  loadChildren: () => import('./landing-page/landing-page/landing-page.module').then(m => m.LandingPageModule),
},
  {
    path: 'method',
    canActivate: [authenticationGuard],
    loadChildren: () => import('./method/method.module').then(m => m.MethodModule),
  },
  {
    path: 'class',
    canActivate: [authenticationGuard],
    loadChildren: () => import('./class/class.module').then(c => c.ClassModule),
  },
    {
    path: 'attribute',
      canActivate: [authenticationGuard],
    loadChildren: () => import('./attribute/attribute.module').then(c => c.AttributeModule),
  },
      {
    path: 'namespace', canActivate: [authenticationGuard],
    loadChildren: () => import('./namespace/namespace.module').then(c => c.NamespaceModule),
  },
  {
    path: 'simulator',
    canActivate: [authenticationGuard],
    loadChildren: () => import('./simulator/simulator.module').then(m => m.SimulatorModule),
  },
  {
    path: 'outputModel',
    canActivate: [authenticationGuard],
    loadChildren: () => import('./outputModel/upload-outputModel.module').then(m => m.UploadOutputModelModule),
  },
  {
    path: 'sesion',
    loadChildren: () => import('./sesion/sesion.module').then(m => m.SesionModule),
  },
  {
  path: 'visitor',
  loadComponent: () => import('./examples/visitor/visitor.component').then(m => m.VisitorComponent)
}
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
