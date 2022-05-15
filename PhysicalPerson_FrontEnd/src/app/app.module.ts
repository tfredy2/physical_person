import { NgModule,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ModalModule } from 'ngb-modal';
import { CookieService } from 'ngx-cookie-service';
import { NgxPaginationModule } from 'ngx-pagination';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { ReportComponent } from './components/report/report.component';
import { PersonComponent } from './components/person/person.component';
import { HttpClient,HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './components/home/home.component';
import { NgxSpinnerModule} from 'ngx-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NewaccountComponent } from './components/newaccount/newaccount.component';
import { NavbarComponent } from './components/navbar/navbar.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ReportComponent,
    PersonComponent,
    HomeComponent,
    NewaccountComponent,
    NavbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgxSpinnerModule,
    FormsModule, ReactiveFormsModule,
    ModalModule,
    NgxPaginationModule,
    ToastrModule.forRoot()   
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
