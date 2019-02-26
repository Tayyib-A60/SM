import { User } from './../models/user.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

@Injectable()
export class UserService {
  constructor(private httpClient: HttpClient) {
  }
  header() {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    let httpOptions = {
      headers: new HttpHeaders({})
    };
    if (currentUser && currentUser.token) {
        httpOptions = {
        headers: new HttpHeaders({
          'Content-Type':  'application/json',
          'Authorization':  `Bearer ${currentUser.token}`
        })
      };
    }
    return httpOptions;
  }
  getAll() {
    return this.httpClient.get('http://localhost:5000/api/skineroVehicles/user', this.header()).toPromise();
  }
  getUser(id: number) {
    return this.httpClient.get('http://localhost:5000/api/skineroVehicles/user' + id, this.header()).toPromise();
  }
  create(user: User) {
    return this.httpClient.post('http://localhost:5000/api/skineroVehicles/user/register', user, this.header());
  }
  update(user: User) {
    return this.httpClient.put('http://localhost:5000/api/skineroVehicles/user', user, this.header());
  }
  delete(id: number) {
    return this.httpClient.delete('http://localhost:5000/api/skineroVehicles/user/' + id, this.header());
  }
}
