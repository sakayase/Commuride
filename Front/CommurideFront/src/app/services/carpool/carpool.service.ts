import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, retry } from 'rxjs';
import { carpoolInterface } from '../../interfaces/carpoolInterface';

@Injectable({
  providedIn: 'root'
})
export class carpoolService {
  private newCarpool$;

  constructor(private http: HttpClient) {
    this.newCarpool$ = new Subject();
  }

  emitNewCarpoolObservable(partialCarpool: Omit<carpoolInterface, 'id'>): void {
    this.newCarpool$.next(partialCarpool);
  }

  getNewCarpoolObservable(): Observable<any> {
    return this.newCarpool$.asObservable();
  }

  loadCarpools(): Observable<carpoolInterface[]> {
    const url = 'http://localhost:5280/api/Carpool/GetCarpools';
    console.log(this.http.get<Array<carpoolInterface>>(url).pipe(retry(1)));
    return this.http.get<Array<carpoolInterface>>(url).pipe(retry(1))
  }
}
