import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VehicleService } from './services/vehicle.service';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { PhotoService } from './services/photo.service';
import { VehicleInterceptor } from './services/vehicle-interceptor.service';
import { VehicleResolver } from './services/vehicle-resolver.service';
import { VehicleComponent } from './vehicle/vehicle.component';
import { AuthGuardService } from './services/authGuard.service';
import { AuthService } from './services/authentication.service';
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
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AdminVehicleListComponent } from './admin-vehicle-list/admin-vehicle-list.component';
import { HomepageComponent } from './homepage/homepage.component';
import { FeaturedVehicleResolver } from './services/featured-vehicle-resolver.service';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

const routes: Routes = [
  {path: 'home', component: HomepageComponent, resolve: {resolvedFeaturedVehicles: FeaturedVehicleResolver}},
  {path: 'publicVehicles', component: VehicleComponent,  resolve: {resolvedVehicles: VehicleResolver}},
  {path: 'newVehicle', component: VehicleEditComponent, canActivate: [AuthGuardService]},
  {path: 'newVehicle/:id', component: VehicleEditComponent, canActivate: [AuthGuardService] },
  {path: 'vehicles', component: AdminVehicleListComponent, canActivate: [AuthGuardService] },
  {path: 'vehicle/photo/:id', component: AddPicturesComponent, canActivate: [AuthGuardService]},
  {path: 'publicVehicles', component: VehicleComponent, resolve: {resolvedVehicles: VehicleResolver}},
  {path: 'vehicle/details/:id', component: VehicleDetailsComponent},
  {path: 'user/registeredUsers', component: RegisteredUsersComponent, canActivate: [AuthGuardService]},
  {path: 'user/login', component: LoginComponent},
  {path: 'user/register', component: RegisterComponent},
  {path: 'user/forgotPassword', component: ForgotPasswordComponent},
  {path: 'user/resetpassword', component: ResetPasswordComponent},
  {path: 'contactus', component: ContactusComponent},
  {path: '**', redirectTo: 'home'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [VehicleService, PhotoService, VehicleResolver, VehicleInterceptor,
    AuthGuardService, AuthService, LogResponseInterceptor, CacheInterceptor,
    ContactFormService, UserService, HttpCacheService,
    {provide: HTTP_INTERCEPTORS, useClass: LogResponseInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: CacheInterceptor, multi: true}]
})
export class AppRoutingModule { }
