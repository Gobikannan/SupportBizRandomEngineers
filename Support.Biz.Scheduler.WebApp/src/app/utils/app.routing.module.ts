import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginGuard } from './login.guard';
import { HomeComponent } from '../home/home.component';
import { EngineersComponent } from '../engineers/engineers.component';
import { ShiftsComponent } from '../shifts/shifts.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'engineers', component: EngineersComponent, canActivate: [LoginGuard] },
  { path: 'shifts', component: ShiftsComponent, canActivate: [LoginGuard] },
  // otherwise redirect to home
  { path: '**', redirectTo: 'shifts' }
];

const rootRouting: ModuleWithProviders = RouterModule.forRoot(routes, {
  useHash: true,
  // enableTracing: true,
});

@NgModule({
  imports: [ rootRouting ],
  exports: [ RouterModule ],
  providers: [LoginGuard]
})

export class AppRoutingModule {

}
