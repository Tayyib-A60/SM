import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/authentication.service';
import { AuthGuardService } from '../services/authGuard.service';
import { Router, ActivatedRoute } from '@angular/router';
import { VehicleResolver } from '../services/vehicle-resolver.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {
  searchString = '';
    constructor(private authService: AuthService,
                private authGuardService: AuthGuardService,
                private router: Router, private route: ActivatedRoute,
                private vehicleResolver: VehicleResolver) {
  }

 isAuthenticated(): boolean {
   return this.authGuardService.isAuthenticated();
 }

 ngOnInit() {
   this.vehicleResolver.searchString = this.searchString;
 }

 collapse() {
  //  this.isExpanded = false;
 }

 toggle() {
  //  this.isExpanded = !this.isExpanded;
 }

 logout() {
   this.authService.logout();
   this.router.navigate(['../publicVehicle'], {relativeTo: this.route});
 }

}
