import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AngMaterialModule } from '../utils/mat.module';
import { ShiftsComponent } from './shifts.component';
import { ShiftsService } from './shifts.service';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    AngMaterialModule
  ],
  declarations: [ ShiftsComponent ],
  exports: [ ShiftsComponent ],
  providers: [ ShiftsService ]
})

export class ShiftsModule { }
