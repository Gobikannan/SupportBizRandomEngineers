import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AngMaterialModule } from '../utils/mat.module';
import { EngineersComponent } from './engineers.component';
import { EngineersService } from './engineers.service';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    AngMaterialModule
  ],
  declarations: [ EngineersComponent ],
  exports: [ EngineersComponent ],
  providers: [ EngineersService ]
})

export class EngineersModule { }
