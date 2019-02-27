import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { ErrorMessage } from '../models/errorHandler.model';
import { environment } from '../../environments/environment';
import { NotifierService } from 'angular-notifier';
import { ActivatedRoute, Router } from '@angular/router';

@Injectable()
export class AuthService {
  // url = environment.url;
  constructor(private httpClient: HttpClient,
              private notifierService: NotifierService,
              private router: Router,
              private route: ActivatedRoute){
              }

  login(email: string, password: string) {
    return this.httpClient.post('http://localhost:5000/api/skineroVehicles/user/authenticate', {email: email, password: password})
    .pipe(
      catchError((err: HttpErrorResponse) => this.handleHttpError(err))
    );
  }
  private handleHttpError(error: HttpErrorResponse) {
    const errorMessage: ErrorMessage = {message: ''};
    errorMessage.message = 'Unable to get';
    return throwError(errorMessage);
  }
  logout() {
    // this.router.navigate(['../../publicVehicles'], {relativeTo: this.route});
    localStorage.removeItem('currentUser');
    this.notifierService.notify('info', 'Logged out');
  }
}
