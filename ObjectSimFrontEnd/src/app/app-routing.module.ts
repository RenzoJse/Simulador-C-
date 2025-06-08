import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { notAuthenticatedGuard } from '../guard/not-authenticated.guard';
import { authenticationGuard } from '../guard/authentication.guard';

const routes: Routes = [
  {
    path: 'inicio',
    canActivate: [notAuthenticatedGuard],
    loadChildren: () => import('./landing-page/landing-page.module').then(m => m.LandingPageModule),
  },
  {
    path: 'method',
    canActivate: [notAuthenticatedGuard],
    loadChildren: () => import('./method/method.module').then(m => m.MethodModule),
  },
  {
    path: 'class',
    canActivate: [notAuthenticatedGuard],
    loadChildren: () => import('./class/class.module').then(c => c.ClassModule),
  },
    {
    path: 'attribute',
      canActivate: [authenticationGuard],
    loadChildren: () => import('./attribute/attribute.module').then(c => c.AttributeModule),
  },
      {
    path: 'namespace',
        canActivate: [authenticationGuard],
    loadChildren: () => import('./namespace/namespace.module').then(c => c.NamespaceModule),
  },
  {
    path: 'simulator',
    canActivate: [authenticationGuard],
    loadChildren: () => import('./simulator/simulator.module').then(m => m.SimulatorModule),
  }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
