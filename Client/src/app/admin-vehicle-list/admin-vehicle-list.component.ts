import { QueryResult } from './../models/queryResult.model';
import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { Vehicle } from '../models/vehicle.model';

@Component({
  selector: 'app-admin-vehicle-list',
  templateUrl: './admin-vehicle-list.component.html',
  styleUrls: ['./admin-vehicle-list.component.css']
})
export class AdminVehicleListComponent implements OnInit {
  vehicles: Vehicle[];
  queryResult: QueryResult = {
    totalItems: 0,
    vehicles: []
  };
  private readonly pageSize = 10;
  query: any = {
    pageSize: this.pageSize
  };
  constructor(private vehicleService: VehicleService) { }

  ngOnInit() {
    this.populateVehicles();
  }
  private populateVehicles() {
    this.vehicleService.getVehicles(this.query).subscribe(queryResult => {
      this.queryResult = queryResult as QueryResult;
      this.vehicles = this.queryResult.vehicles as Vehicle[];
    });
  }
  onPageChange(page: number) {
    this.query.page = page;
    this.populateVehicles();
  }
}
