import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

import { PhotoService } from '../services/photo.service';
import { Vehicle } from '../models/vehicle.model';
import { Photo } from '../models/Photo.model';
import { VehicleService } from '../services/vehicle.service';

@Component({
  selector: 'app-vehicle-details',
  templateUrl: './vehicle-details.component.html',
  styleUrls: ['./vehicle-details.component.css']
})
export class VehicleDetailsComponent implements OnInit {
  index: number;
  vehicle: Vehicle;
  photos: Photo[];
  constructor(private route: ActivatedRoute, private photoService: PhotoService, private vehicleService: VehicleService) { }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.index = +params['id'];
    });
    this.vehicleService.getVehicle(this.index).subscribe(vehicle => {
      this.vehicle = vehicle as Vehicle;
    }, error => {
    });
    this.photoService.getPhotos(this.index).subscribe(res => {
      this.photos = res as Photo[];
    });
  }

}
