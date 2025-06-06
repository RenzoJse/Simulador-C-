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
  },
  {
    path: 'class',
    loadChildren: () => import('./class/class.module').then(c => c.ClassModule),
  },
    {
    path: 'attribute',
    loadChildren: () => import('./attribute/attribute.module').then(c => c.AttributeModule),
  },
      {
    path: 'namespace',
    loadChildren: () => import('./namespace/namespace.module').then(c => c.NamespaceModule),
  },
  {
    path: 'simulator',
    loadChildren: () => import('./simulator/simulator.module').then(m => m.SimulatorModule),
  }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
