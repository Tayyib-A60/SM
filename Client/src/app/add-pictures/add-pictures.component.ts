import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpEvent, HttpEventType } from '@angular/common/http';
// import { ToastrService } from 'ngx-toastr'

import { VehicleService } from '../services/vehicle.service';
import { Vehicle } from '../models/vehicle.model';
import { PhotoService } from '../services/photo.service';
import { Photo } from '../models/Photo.model';
import { AlertifyService } from '../services/alertify.service';


@Component({
  selector: 'app-add-pictures',
  templateUrl: './add-pictures.component.html',
  styleUrls: ['./add-pictures.component.css']
})
export class AddPicturesComponent implements OnInit {
  vehicleId: number;
  vehicle: Vehicle;
  @ViewChild('fileInput') fileInput: ElementRef;
  photos: Photo[];
  currentPage = 'details';
  detailsTab = true;
  photosTab = false;
  progress: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    // private toastr: ToastrService,
    private photoService: PhotoService,
    private vehicleService: VehicleService,
    private alert: AlertifyService) {
      route.params.subscribe(p => {
        this.vehicleId = +p['id'];
        if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
          router.navigate(['/vehicles']);
          return;
        }
      });
     }

  ngOnInit() {
    this.getVehicle();
    this.photoService.getPhotos(this.vehicleId).subscribe(photos =>
      this.photos = photos as Photo[]
      );
  }
  private getVehicle() {
    this.vehicleService.getVehicle(this.vehicleId)
      .subscribe(
        v => this.vehicle = v as Vehicle,
        err => {
          if (err.status === 404) {
            this.router.navigate(['/vehicles']);
            return;
          }
        });
  }
  changePage(page) {
    this.currentPage = page;
    if (page === 'details') {
      this.detailsTab = true;
      this.photosTab = false;
    }
    if (page === 'photos') {
      this.detailsTab = false;
      this.photosTab = true;
    }
  }
  deleteVehicle() {
    if (confirm('Are you sure?')) {
      this.vehicleService.deleteVehicle(this.vehicle.id)
        .subscribe(x => {
          this.alert.success('Vehicle delete was successful');
          console.log(x);
          this.router.navigate(['../../../vehicles'], {relativeTo: this.route});
        }, (err) => {
          console.log(err);
        });
    }
  }
  deletePhoto(id: number) {
    this.photoService.deletePhoto(id, this.vehicle.id).subscribe(res => {
      const index = this.photos.findIndex(p => p.id === id && p.vehicleId === this.vehicle.id);
      this.photos.splice(index, 1);
      this.alert.success(`Photo with id ${id}  succesfully removed`);
      // this.toastr.success(`Photo with id ${id}  succesfully removed`, 'Success');
      console.log(res);
      console.log(this.photos);
    }, (err) => {
      console.log(err);
    });
  }
  edit() {
    this.router.navigate(['../../form/' + this.vehicleId], {relativeTo: this.route});
  }
  onPhotoUpload() {
    let nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    const file = nativeElement.files[0];
    nativeElement = '' as any;
    this.photoService.uploadPhoto(this.vehicleId, file)
    .subscribe((event: HttpEvent<any>) => {
        switch (event.type) {
          case HttpEventType.Sent:
             console.log('Request sent');
             this.alert.success('Photo uploaded succesfully');
            //  this.toastr.success('Photo uploaded succesfully', 'Success');
            break;
          case HttpEventType.UploadProgress:
            const percentage = Math.round(100 * event.loaded / event.total);
            this.progress = percentage;
            break;
          case HttpEventType.Response:
            this.photos.push(event.body);
            this.alert.success('Photo uploaded succesfully');
            // this.toastr.success('Photo uploaded succesfully', 'Success');
        }
    }, err => {
      console.log('Error', err);
      console.log(err.error);
      // this.toastr.error(err.error, 'Error');
    });
  }
}
