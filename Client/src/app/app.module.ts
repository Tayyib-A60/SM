import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NotifierModule } from 'angular-notifier';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { VehicleComponent } from './vehicle/vehicle.component';
import { VehicleItemComponent } from './vehicle/vehicle-item/vehicle-item.component';
import { VehicleService } from './services/vehicle.service';
import { PhotoService } from './services/photo.service';
import { VehicleInterceptor } from './services/vehicle-interceptor.service';
import { VehicleResolver } from './services/vehicle-resolver.service';
import { FooterComponent } from './footer/footer.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthService } from './services/authentication.service';
import { AuthGuardService } from './services/authGuard.service';
import { LogResponseInterceptor } from './services/logResponseInterceptor';
import { CacheInterceptor } from './services/cache.interceptor';
import { ContactFormService } from './services/contactForm.service';
import { UserService } from './services/user.service';
import { HttpCacheService } from './services/http-cache.service';
import { ContactusComponent } from './contactus/contactus.component';
import { RegisterComponent } from './register/register.component';
import { AddPicturesComponent } from './add-pictures/add-pictures.component';
import { LoginComponent } from './login/login.component';
import { RegisteredUsersComponent } from './registered-users/registered-users.component';
import { VehicleEditComponent } from './vehicle-edit/vehicle-edit.component';
import { VehicleDetailsComponent } from './vehicle-details/vehicle-details.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminVehicleListComponent } from './admin-vehicle-list/admin-vehicle-list.component';
import { PaginationComponent } from './shared/pagination/pagination.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    VehicleComponent,
    VehicleItemComponent,
    FooterComponent,
    ContactusComponent,
    LoginComponent,
    VehicleDetailsComponent,
    AdminVehicleListComponent,
    VehicleEditComponent,
    RegisterComponent,
    RegisteredUsersComponent,
    AddPicturesComponent,
    PaginationComponent,
    VehicleDetailsComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    NotifierModule.withConfig({
      position: {
        horizontal: {
          position: 'right',
          distance: 25
        },
        vertical: {
          position: 'top',
          distance: 12,
          gap: 10
        }
      }
    }),
    MDBBootstrapModule.forRoot()
  ],
  providers: [VehicleService, PhotoService, VehicleResolver, VehicleInterceptor,
     AuthGuardService, AuthService, LogResponseInterceptor, CacheInterceptor,
     ContactFormService, UserService, HttpCacheService,
     {provide: HTTP_INTERCEPTORS, useClass: LogResponseInterceptor, multi: true},
  {provide: HTTP_INTERCEPTORS, useClass: CacheInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
