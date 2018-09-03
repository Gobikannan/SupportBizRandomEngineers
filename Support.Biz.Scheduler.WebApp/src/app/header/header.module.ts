import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header.component';
import { RouterModule } from '@angular/router';
import { AngMaterialModule } from '../utils/mat.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    AngMaterialModule
  ],
  declarations: [HeaderComponent],
  exports: [HeaderComponent]
})

export class HeaderModule { }
