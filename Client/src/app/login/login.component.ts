import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../services/authentication.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  returnUrl: string;
  user = {
    email: '',
    password: ''
  };
  constructor(private authService: AuthService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.authService.logout();
    this.initializeForm();
    this.returnUrl = this.route.snapshot.queryParams['../registeredUsers'] || '/';
  }
  private initializeForm() {
    const email = '';
    const password = '';
    this.loginForm = new FormGroup({
      email: new FormControl(email, [Validators.required, Validators.email]),
      password: new FormControl(password, [Validators.required])
    });
  }
  login() {
    this.user = this.loginForm.value;
    this.authService.login(this.user.email, this.user.password).subscribe(res => {
      const user = res;
      if (user && user['token']) {
        localStorage.setItem('currentUser', JSON.stringify(user));
      }
      this.router.navigate(['user/registeredUsers']);
    }, error => {
      console.log(error);
    });
    this.loginForm = new FormGroup({});
  }

}
