import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HeaderModule } from './header/header.module';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './utils/app.routing.module';
import { AngMaterialModule } from './utils/mat.module';
import { HomeModule } from './home/home.module';
import { EngineersModule } from './engineers/engineers.module';
import { ShiftsModule } from './shifts/shifts.module';

import { MAT_DATE_LOCALE} from '@angular/material/core';

import 'hammerjs';

import { loadProgressBar } from 'axios-progress-bar';

loadProgressBar();

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    AngMaterialModule,
    HeaderModule,
    HomeModule,
    EngineersModule,
    ShiftsModule
  ],
  providers: [ {provide: MAT_DATE_LOCALE, useValue: 'en-GB'}, ],
  bootstrap: [AppComponent]
})
export class AppModule { }
