import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AngMaterialModule } from '../utils/mat.module';
import { HomeComponent } from './home.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    AngMaterialModule
  ],
  declarations: [HomeComponent],
  exports: [HomeComponent]
})

export class HomeModule { }
