import {Injectable} from '@angular/core';
import {Category} from '../../shared/models/category';
import {BehaviorSubject} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs/internal/Observable';


@Injectable()
export class CategoryService {

  private categorySubject = new BehaviorSubject([]);
  private categories: Category[] = [];
  private categorySubjectSelectAll = new BehaviorSubject(true);

  constructor(private http: HttpClient) {
  }

  getById(id: string): Category {
    const category = this.categories.find(x => x.id === id);
    if (category) {
      return category;
    }

    return new Category();
  }

  saveChanges(categories: Category[]): boolean {
    this.categories = categories;
    this.http.post(environment.URL + '/categories', categories).subscribe(() => {
      this.getAll();
      this.categorySubjectSelectAll.next(true);
    });

    return true;
  }

  getAll(): Observable<Category[]> {
    this.http.get<Category[]>(environment.URL + '/categories/list').subscribe(response => {
      this.categories = response;
      this.categorySubject.next(this.categories);
    });

    return this.categorySubject.asObservable();
  }

  getLoadedCategories(): Observable<Category[]> {
    return this.categorySubject.asObservable();
  }

  getSubjectSelectAll(): Observable<boolean> {
    return this.categorySubjectSelectAll.asObservable();
  }

  selectAll(select: boolean): void {
    this.categorySubjectSelectAll.next(select);
  }
}
