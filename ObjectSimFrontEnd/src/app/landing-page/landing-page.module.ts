import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { LandingPageRoutingModule } from './landing-page-routing.module';
import { InfoGuideComponent } from "../../components/info-guides/info-guide.component";

@NgModule({
  declarations: [
    LandingPageComponent,
  ],
  imports: [
    CommonModule,
    LandingPageRoutingModule,
    InfoGuideComponent
  ]
})
export class LandingPageModule { }
