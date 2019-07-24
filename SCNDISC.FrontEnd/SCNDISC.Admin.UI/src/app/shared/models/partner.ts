import {WebAddress} from './webAddress';
import {Contact} from './contact';
import {Category} from './category';

export class Partner {
  public webAddresses: WebAddress[];
  public contacts: Contact[];
  public categories: Category[];
  public name_Ru: string;
  public name_En: string;
  public description_Ru: string;
  public description_En: string;
  public image: string;
  public logo: string;
  public gallery: string[];
  public comment: string;
  public discount: number;
  public selectDiscount: string;
  public id: string;

  constructor() {
      this.categories = [];
      this.webAddresses = [];
      const contact = new Contact();
      this.contacts = [];
      this.contacts.push(contact);
      this.selectDiscount = '0';
      this.discount = 0;
      this.gallery = [];
  }
}
