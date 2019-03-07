import { Component } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, Event, NavigationCancel, NavigationError } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  showLoadingIndicator = true;
  constructor(private router: Router) {
      this.router.events.subscribe((routerEvent: Event) => {
      if (routerEvent instanceof NavigationStart) {
        this.showLoadingIndicator = true;
        console.log('navigation started');
      }
      if (routerEvent instanceof NavigationEnd || routerEvent instanceof NavigationCancel || routerEvent instanceof NavigationError) {
        this.showLoadingIndicator = false;
        console.log('navigation ended');
      }
    });
  }
}
