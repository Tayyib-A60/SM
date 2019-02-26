import { ToastrService } from 'ngx-toastr';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { Router, ActivatedRoute } from '@angular/router';
import { User } from '../models/user.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registrationForm: FormGroup;
  user: User;
  constructor(private userService: UserService,
              private router: Router,
              private route: ActivatedRoute,
              private toastr: ToastrService) { }

  ngOnInit() {
    this.initializeForm();
  }
  private initializeForm() {
    const name = '';
    const email = '';
    const password = '';
    this.registrationForm = new FormGroup({
      name: new FormControl(name, [Validators.required]),
      email: new FormControl(email, [Validators.required, Validators.email]),
      password: new FormControl(password, [Validators.required])
    });
  }
  register() {
    this.userService.create(this.registrationForm.value).subscribe(user => {
      this.user = user as User;
      console.log(user);
      this.toastr.success(`User with ${this.user.email} registered successfully`, 'Registered');
    }, error => {
    });
    this.registrationForm = new FormGroup({});
    this.router.navigate(['../login'], {relativeTo: this.route});
  }

}
