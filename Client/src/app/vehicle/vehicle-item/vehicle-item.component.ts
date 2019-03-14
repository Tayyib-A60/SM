import { Router, ActivatedRoute } from '@angular/router';
import { Vehicle } from './../../models/vehicle.model';
import { Component, OnInit, Input } from '@angular/core';
import { PhotoService } from '../../services/photo.service';
import { Photo } from '../../models/Photo.model';

@Component({
  selector: 'app-vehicle-item',
  templateUrl: './vehicle-item.component.html',
  styleUrls: ['./vehicle-item.component.scss']
})
export class VehicleItemComponent implements OnInit {
  @Input() vehicle: Vehicle;
  @Input() index: number;
  photos: Photo[] = [];
  constructor(private router: Router, private route: ActivatedRoute, private photoService: PhotoService) { }

  ngOnInit() {
    this.getPhoto();
  }

  seeDetails() {
    this.router.navigate(['../vehicle/details/' + this.vehicle.id], {relativeTo: this.route});
  }
  private getPhoto() {
    this.photoService.getPhotos(this.vehicle.id).subscribe(res => {
      this.photos = res as Photo[];
    });
  }

}
