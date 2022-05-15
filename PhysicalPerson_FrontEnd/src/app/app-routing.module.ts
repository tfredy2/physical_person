import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { PersonComponent } from './components/person/person.component';
import { ReportComponent } from './components/report/report.component';
import { AuthGuard } from './security/auth.guard';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'home',pathMatch:'full', component:  HomeComponent,canActivate:[AuthGuard] },
  { path: 'person',pathMatch:'full', component:  PersonComponent,canActivate:[AuthGuard]  },
  { path: 'report',pathMatch:'full', component:  ReportComponent,canActivate:[AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
