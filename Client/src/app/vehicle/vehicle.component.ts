import { ErrorMessage } from './../models/errorHandler.model';
import { ActivatedRoute } from '@angular/router';
import { Vehicle } from './../models/vehicle.model';
import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { QueryResult } from '../models/queryResult.model';
import { HttpClient } from '@angular/common/http';
import { VehicleResolver } from '../services/vehicle-resolver.service';

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.scss']
})

export class VehicleComponent implements OnInit {
  vehicles: Vehicle[] = [];
  notFound = false;
  private readonly pageSize = 8;
  queryResult: QueryResult = {
    totalItems: 0,
    vehicles: []
  };
  query: any = {
    pageSize: 8,
    searchString: ''
  };

  constructor(private vehicleService: VehicleService,
              private route: ActivatedRoute,
              private http: HttpClient,
              private vehicleResolver: VehicleResolver) { }

  ngOnInit() {
    this.getVehicles();
  }
  private getVehicles() {
    const resolveData: QueryResult | ErrorMessage = this.route.snapshot.data['resolvedVehicles'];
    this.queryResult = resolveData as QueryResult;
    this.vehicles = this.queryResult.vehicles as Vehicle[];
    if (resolveData instanceof ErrorMessage) {
    } else if (resolveData instanceof Vehicle) {
    }
  }
  private getVehiclees() {
    this.vehicleService.getVehicles(this.query).subscribe(queryResult => {
      this.queryResult = queryResult as QueryResult;
      this.vehicles = this.queryResult.vehicles;
    });
  }
  onPageChange(page: number) {
    this.query.page = page;
    this.getVehiclees();
  }
  searchDB() {
    this.vehicleResolver.query.searchString = this.query.searchString;
    this.vehicleService.getVehicles(this.query, this.query.searchString).subscribe(queryResult => {
        this.queryResult = queryResult as QueryResult;
        this.vehicles = this.queryResult.vehicles;
        if (this.vehicles.length === 0) {
            this.notFound = true;
          }
        if (this.vehicles.length > 0) {
            this.notFound = false;
          }
      }, (err) => {
      }, () => {
        this.vehicleResolver.query.searchString = '';
        this.query.searchString = '';
      });
    }

}
