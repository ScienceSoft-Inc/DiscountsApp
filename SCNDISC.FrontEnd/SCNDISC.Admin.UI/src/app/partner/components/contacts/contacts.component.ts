import {Component, Input, OnInit} from '@angular/core';
import {Contact} from '../../../shared/models/contact';
import {Partner} from '../../../shared/models/partner';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css']
})
export class ContactsComponent implements OnInit {

  @Input() partner: Partner;
  public display: boolean;

  constructor() {
  }

  ngOnInit(): void {
    if (this.partner.contacts && this.partner.contacts.length < 2) {
      this.display = false;
    } else {
      this.display = true;
    }
  }

  onClickedAddNewContact(): void {
    const contact = new Contact();
    this.partner.contacts.push(contact);
    if (this.partner.contacts.length > 1) {
      this.display = true;
    }
  }

  removeContact() {
    this.partner.contacts.pop();
    if (this.partner.contacts.length < 2) {
      this.display = false;
    }
  }
}
