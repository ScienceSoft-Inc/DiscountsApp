import {Component, OnDestroy, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {MenuService} from '../../core/services/menu.service';
import {Subscription} from 'rxjs/internal/Subscription';
import {PartnerService} from '../../core/services/partner.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit, OnDestroy {

  private menuSubscription: Subscription;
  public menuDisplay: boolean;

  constructor(private router: Router, private menuService: MenuService, private partnerService: PartnerService) {
  }

  ngOnInit(): void {
    this.menuSubscription = this.menuService.getMenuDisplay().subscribe((displayMenu: boolean) => {
      this.menuDisplay = displayMenu;
    });
  }

  onClickedMenu(event): void {
    event.stopPropagation();
    this.menuService.showMenu(!this.menuDisplay);
  }

  onClickedManageCategories(): void {
    this.menuService.offShowMenu();
    this.router.navigate(['categories']);
  }

  onClickedFeedback(): void {
    this.menuService.offShowMenu();
    this.router.navigate(['feedback']);
  }

  onClickedNewPartner(): void {
    this.menuService.offShowMenu();
    this.router.navigate(['partner', this.partnerService.pathNewPartner]);
  }

  ngOnDestroy(): void {
    this.menuSubscription.unsubscribe();
  }
}
