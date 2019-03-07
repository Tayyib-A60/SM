import { QueryResult } from './../models/queryResult.model';
import { catchError } from 'rxjs/operators';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { VehicleService } from './vehicle.service';

@Injectable()
export class FeaturedVehicleResolver implements Resolve<QueryResult> {
  private readonly pageSize = 16;
  query: any = {
    pageSize: this.pageSize
  };
  constructor(private vehicleService: VehicleService) {
  }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<QueryResult> {
    return this.vehicleService.getFeaturedVehicles(this.query)
    .pipe(
      catchError(err => of(err))
    );
  }
}
