import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { User } from '../models/user.model';
import { NotifierService } from 'angular-notifier';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  public Editor = ClassicEditor;
  emailForm: FormGroup;
  email: string;
  user: User;
  disableButton = false;
  constructor(private userService: UserService,
              private notifierService: NotifierService) { }

  ngOnInit() {
    this.initializeForm();
  }
  getUser() {
    this.userService.getUserByEmail(this.emailForm.controls['email'].value).subscribe(user => {
        this.disableButton = true;
        this.user = user as User;
      }, err => {
        this.notifierService.notify('error', 'User with specified email not found');
      }, () => {
        this.userService.forgotPassword(this.user).subscribe(user => {
        }, () => {
        },
        () => {
          this.notifierService.notify('success', 'A link to reset your password has been sent to your email');
          this.disableButton = false;
          this.initializeForm();
        });
      }
    );
  }
  private initializeForm() {
    const email = '';
    this.emailForm = new FormGroup({
      email: new FormControl(email, [Validators.required, Validators.email])
    });
  }
  send() {
    this.userService.forgotPassword(this.emailForm.controls['email'].value).subscribe(user => {
    }, error => {
    },
    () => {
    });
    this.emailForm = new FormGroup({});
  }
}
