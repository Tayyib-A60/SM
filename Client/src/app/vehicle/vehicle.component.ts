import { ErrorMessage } from './../models/errorHandler.model';
import { ActivatedRoute } from '@angular/router';
import { Vehicle } from './../models/vehicle.model';
import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { QueryResult } from '../models/queryResult.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.scss']
})

export class VehicleComponent implements OnInit {
  vehicles: Vehicle[] = [];
  private readonly pageSize = 8;
  queryResult: QueryResult = {
    totalItems: 0,
    vehicles: []
  };
  query: any = {
    pageSize: 8
  };

  constructor(private vehicleService: VehicleService, private route: ActivatedRoute, private http: HttpClient) { }

  ngOnInit() {
    this.getVehicles();
  }
  private getVehicles() {
    const resolveData: QueryResult | ErrorMessage = this.route.snapshot.data['resolvedVehicles'];
    this.queryResult = resolveData as QueryResult;
    // console.log(this.queryResult);
    this.vehicles = this.queryResult.vehicles as Vehicle[];
    if (resolveData instanceof ErrorMessage) {
      // console.log(resolveData.message);
    } else if (resolveData instanceof Vehicle) {
      // console.log(resolveData);
      // console.log(this.vehicles);
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

}
