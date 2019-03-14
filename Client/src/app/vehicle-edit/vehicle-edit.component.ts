import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router, Params } from '@angular/router';

import { Vehicle } from './../models/vehicle.model';
import { VehicleService } from '../services/vehicle.service';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-vehicle-edit',
  templateUrl: './vehicle-edit.component.html',
  styleUrls: ['./vehicle-edit.component.css', './vehicle-edit.component.scss']
})
export class VehicleEditComponent implements OnInit {
  vehicleForm: FormGroup =  new FormGroup({});
  editMode: boolean;
  index: number;
  vehicle: Vehicle;
  constructor(private vehicleService: VehicleService,
              private route: ActivatedRoute,
              private router: Router,
              private notifierService: NotifierService) { }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.index = +params['id'];
      this.editMode = params['id'] != null;
    });
    this.initializeForm();
  }
  private getVehicle() {
    this.vehicleService.getVehicle(this.index).subscribe(vehicle => {
      this.vehicle = vehicle as Vehicle;
    }, error => {
    });
  }
  private initializeForm() {
    const name = '';
    const description = '';
    const costPrice = 0;
    const sellingPrice = 0;
    const discount = 0;
    const featured = false;
    if (this.editMode) {
      this.vehicleService.getVehicle(this.index).subscribe(vehicle => {
        this.vehicle = vehicle as Vehicle;
      }, err => {
      }, () => {
          this.vehicleForm = new FormGroup({
            name: new FormControl(this.vehicle.name, Validators.required),
            description: new FormControl(this.vehicle.description, Validators.required),
            costPrice: new FormControl(this.vehicle.costPrice, [Validators.required, Validators.pattern(/^[1-9]+[0-9]*$/)]),
            sellingPrice: new FormControl(this.vehicle.sellingPrice, [Validators.required, Validators.pattern(/^[1-9]+[0-9]*$/)]),
            discount: new FormControl(this.vehicle.discount),
            featured: new FormControl(this.vehicle.featured)
          });
      });
    } else {
      this.vehicleForm =  new FormGroup({
        name: new FormControl(name, Validators.required),
        description: new FormControl(description, Validators.required),
        costPrice: new FormControl(costPrice, [Validators.required,  Validators.pattern(/^[1-9]+[0-9]*$/)]),
        sellingPrice: new FormControl(sellingPrice, [Validators.required,  Validators.pattern(/^[1-9]+[0-9]*$/)]),
        discount: new FormControl(discount),
        featured: new FormControl(featured)
      });
    }
  }
  onSubmit() {
    if (this.editMode) {
      this.vehicleService.updatevehicle(this.vehicle.id, this.vehicleForm.value).subscribe(vehicle => {
        this.notifierService.notify('success', 'Update was successful');
      }, (err) => {
        this.notifierService.notify('error', 'Update was not successful');
      });
      this.vehicleForm = new FormGroup({});
    } else {
      this.vehicleService.createVehicle(this.vehicleForm.value).subscribe(res => {
        this.notifierService.notify('success', 'Successful');
      });
      this.vehicleForm = new FormGroup({});
    }
  }
  clear() {
    this.initializeForm();
  }
  delete() {
    if (confirm('Are you sure?')) {
      this.vehicleService.deleteVehicle(this.vehicle.id).subscribe(response => {
        this.notifierService.notify('success', 'Vehicle deleted successfully');
      }, (err) => {
        this.notifierService.notify('error', 'Vehicle delete not successful');
      });
    }
  }
}
