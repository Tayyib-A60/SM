import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { Vehicle } from '../models/vehicle.model';
import { QueryResult } from '../models/queryResult.model';
import { ErrorMessage } from '../models/errorHandler.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']
})
export class HomepageComponent implements OnInit {
  vehicles: Vehicle[] = [];
  featuredVehicles: Vehicle[] = [];
  private readonly pageSize = 20;
  queryResult: QueryResult = {
    totalItems: 0,
    vehicles: []
  };
  query: any = {
    pageSize: this.pageSize
  };
  constructor(private vehicleService: VehicleService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.getVehicles();
  }
  private getVehicles() {
    const resolveData: QueryResult | ErrorMessage = this.route.snapshot.data['resolvedFeaturedVehicles'];
    this.queryResult = resolveData as QueryResult;
    this.vehicles = this.queryResult.vehicles as Vehicle[];
  }
}
