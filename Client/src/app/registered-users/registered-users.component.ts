import { ContactFormService } from './../services/contactForm.service';
import { User } from './../models/user.model';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { ContactForm } from '../models/contactForm.model';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-registered-users',
  templateUrl: './registered-users.component.html',
  styleUrls: ['./registered-users.component.css']
})

export class RegisteredUsersComponent implements OnInit {
  users: User[] = [];
  currentUser: User = <User> { };
  contacts: ContactForm[] = [];

  constructor(private userService: UserService, private contactService: ContactFormService,
    private router: Router, private route: ActivatedRoute) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.users = [];
  }

  ngOnInit() {
    console.log(this.users);
    this.getUsers();
    this.getContacts();
  }
  deleteUser(id: number) {
    this.userService.delete(id).subscribe(res => {
      const index = this.users.findIndex(user => user.id === id);
      this.users.splice(index, 1);
    }, error => {
      console.log(error);
    });
  }
  editUser() {
    this.router.navigate(['../register'], {relativeTo: this.route});
  }
  private getUsers() {
    this.userService.getAll().then(users => {
      this.users = users as User[];
    });
  }
  private getContacts() {
    this.contactService.getAllContacts().then(contacts => {
      this.contacts = contacts as ContactForm[];
    });
  }
}
