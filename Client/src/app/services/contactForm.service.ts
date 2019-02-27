import { ContactForm } from './../models/contactForm.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable()
export class ContactFormService {
  url = environment.url;

  constructor(private httpClient: HttpClient) {
  }
  createContactForm(contact: ContactForm) {
    return this.httpClient.post(this.url + '/api/skineroVehicles/contactUs', contact);
  }
  getAllContacts() {
    return this.httpClient.get(this.url + '/api/skineroVehicles/contactUs').toPromise();
  }
}
