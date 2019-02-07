import { Component, OnInit } from '@angular/core';
import {LanguageService} from "../../core/services/language.service";

@Component({
  selector: 'app-error-server-modal',
  templateUrl: './warning.component.html',
  styleUrls: ['./warning.component.less']
})
export class WarningComponent implements OnInit {

  constructor(public translate: LanguageService) { }

  ngOnInit(): void {
  }

}
