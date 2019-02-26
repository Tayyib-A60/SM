import { ErrorMessage } from './../models/errorHandler.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Vehicle } from '../models/vehicle.model';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Observable } from 'rxjs';
// import { environment } from 'src/environments/environment';

@Injectable()
export class VehicleService {
  // url = environment.url;

  constructor(private httpClient: HttpClient) {
  }

  getValues() {
    return this.httpClient.get('http://localhost:5000/api/weather');
  }

  getVehicles(filter?: any) {
    return this.httpClient.get('http://localhost:5000/api/skineroVehicles'  + '?' + this.toQueryString(filter))
    .pipe(
      catchError((error: HttpErrorResponse) => this.handleError(error))
    );
  }
  private toQueryString(obj) {
    const parts = [];
    for (const property in obj) {
      const value = obj[property];
      if (value != null && value !== undefined) {
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
      }
    }
    return parts.join('&');
  }
  createVehicle(vehicle: Vehicle) {
    return this.httpClient.post('http://localhost:5000/api/skineroVehicles', vehicle);
  }
  getVehicle(id: number) {
    return this.httpClient.get('http://localhost:5000/api/skineroVehicles/' + id)
    .pipe(
      catchError((error: HttpErrorResponse) => this.handleError(error))
    );
  }
  updatevehicle(id: number, vehicle: Vehicle) {
    return this.httpClient.put('http://localhost:5000/api/skineroVehicles/' + id, vehicle);
  }
  deleteVehicle(id: number): Observable<any> {
    return this.httpClient.delete<any>('http://localhost:5000/api/skineroVehicles/' + id);
  }
  private handleError(error: HttpErrorResponse) {
    const errorMessage: ErrorMessage = {message: ''};
    errorMessage.message = 'Unable to get';
    return throwError(errorMessage);
  }
}
