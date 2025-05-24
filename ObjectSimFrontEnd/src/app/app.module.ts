import { NgModule } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from "@angular/common";
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { ReactiveFormsModule } from '@angular/forms';
import { LandingPageComponent } from './landing-page/landing-page/landing-page.component';

import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { CreateMethodComponent } from './components/methods/create-method/create-method.component'; 

@NgModule({
    declarations: [AppComponent, CreateMethodComponent],
    imports: [BrowserModule, CommonModule, FormsModule, ReactiveFormsModule, AppRoutingModule, 
        LandingPageComponent, HeaderComponent, FooterComponent],
    providers: [provideHttpClient()],
    bootstrap: [AppComponent]
})
export class AppModule { }
