import { NgModule } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from "@angular/common";
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { ReactiveFormsModule } from '@angular/forms';
import { LandingPageComponent } from './landing-page/landing-page/landing-page.component';

@NgModule({
    declarations: [AppComponent, LandingPageComponent],
    imports: [BrowserModule, CommonModule, FormsModule, ReactiveFormsModule, AppRoutingModule],
    providers: [provideHttpClient()],
    bootstrap: [AppComponent]
})
export class AppModule { }
