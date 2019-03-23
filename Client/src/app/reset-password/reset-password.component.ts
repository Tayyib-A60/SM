import { UserService } from './../services/user.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { User } from '../models/user.model';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NotifierService } from 'angular-notifier';

const jwthelper = new JwtHelperService();
@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm : FormGroup = new FormGroup({});
  user: User = <User>{};
  payload: any;
  expired: boolean;
  email: string;
  disableButton = false;
  constructor(private userService: UserService,
              private route: ActivatedRoute,
              private router: Router,
              private notifierService: NotifierService) { }

  ngOnInit() {
    // console.log(this.route.snapshot.queryParams['token']);
    this.route.queryParams.subscribe(res => {
      const token = res['token'];
      this.payload = jwthelper.decodeToken(token);
      this.expired = jwthelper.isTokenExpired(token);
      if (this.expired) {
        this.notifierService.notify('error', 'Your token is expired, repeat the process to get another')
        this.router.navigate(['../forgotPassword'], {relativeTo: this.route});
      } else {
        this.email = this.payload['nameid'];
        this.user.email = this.payload['nameid'];
        this.user.name = this.payload['unique_name'];
        this.user.id = this.payload['groupsid'];
        this.user.role = this.payload['role'];
      }
    }, () => {
      this.notifierService.notify('error',"Cannot get user details at the moment, please try again later");
    }, () => {
    });
    
    this.initializeForm();
  }
  private initializeForm() {
    const email = '';
    const password = '';
    this.resetPasswordForm = new FormGroup({
      email: new FormControl({value:this.email, disabled: true}, [Validators.required, Validators.email]),
      password: new FormControl(password, [Validators.required]),
      verifyPassword: new FormControl(password, [Validators.required])
    });
  }
  reset() {
    this.user.password = this.resetPasswordForm.controls['password'].value;
    this.userService.update(this.user).subscribe(user => {
      this.disableButton = true;
    }, () => {
    }, () => {
      this.disableButton = false;
      this.notifierService.notify('success', 'Password reset was successfull');
      this.router.navigate(['../login'], {relativeTo: this.route});
    });
    this.resetPasswordForm = new FormGroup({});
  }
}
