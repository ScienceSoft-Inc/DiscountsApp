import {Component, OnInit, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {PartnerDetailsComponent} from '../partner-details/partner-details.component';
import {PartnerService} from '../../core/services/partner.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-partner-open',
  templateUrl: './partner-dialog.component.html',
  styleUrls: ['./partner-dialog.component.css']
})
export class PartnerDialogComponent implements OnInit {

  constructor(private route: ActivatedRoute, private partnerService: PartnerService,
              private viewRef: ViewContainerRef, private modalService: NgbModal) {
  }

  ngOnInit(): void {
    setTimeout(() => {
      const partner = this.route.snapshot.data.data;
      const modalRef = this.modalService.open(PartnerDetailsComponent, {backdrop: 'static'});
      modalRef.componentInstance.partner = partner;
    });
  }

}
