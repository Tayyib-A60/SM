import { ContactFormService } from './../services/contactForm.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-contactus',
  templateUrl: './contactus.component.html',
  styleUrls: ['./contactus.component.css', './contactus.component.scss']
})
export class ContactusComponent implements OnInit {
  contactForm: FormGroup;
  buttonStatus = 'Submit';

  constructor(private contactService: ContactFormService, private notifierService: NotifierService) { }

  ngOnInit() {
    this.initializeContactForm();
  }

  private initializeContactForm() {
    const name = '';
    const email = '';
    const phone = '';
    const message = '';
    this.contactForm =  new FormGroup({
      name: new FormControl(name, Validators.required),
      email: new FormControl(email, [Validators.required, Validators.email]),
      phone: new FormControl(phone, [Validators.required]),
      message: new FormControl(message)
    });
  }
  onSubmit() {
    this.buttonStatus = 'Submitting';
    this.contactService.createContactForm(this.contactForm.value).subscribe(res => {
     }, (err) => {
      this.notifierService.notify('error', `An unexpected error occured, Please try again`);
     }, () => {
      this.notifierService.notify('success',  `Sent, We'll be in touch shortly`);
      this.buttonStatus = 'Submit';
     });
     this.initializeContactForm();
  }

}
