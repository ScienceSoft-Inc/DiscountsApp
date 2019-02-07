import {Component, OnInit} from '@angular/core';
import {FilterService} from '../filter.service';

@Component({
  selector: 'app-filter-by-name',
  templateUrl: './filter-by-name.component.html',
  styleUrls: ['./filter-by-name.component.css']
})
export class FilterByNameComponent implements OnInit {

  public filterName: string;

  constructor(private filterService: FilterService) { }

  ngOnInit() {
  }

  onChangedFilterName(): void {
    this.filterService.setFilterName(this.filterName);
    this.filterService.applyFilterByName(this.filterName);
  }
}
