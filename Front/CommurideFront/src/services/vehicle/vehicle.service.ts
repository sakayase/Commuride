import { Injectable } from '@angular/core';
import VehicleInterface  from '../../interfaces/vehicleInterface';
import { Observable, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { retry } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private newVehicle$;

  constructor(private http: HttpClient) {
    this.newVehicle$ = new Subject();
  }

  emitNewVehicleObservable(partialVehicle: Omit<VehicleInterface, 'id'>): void {
    this.newVehicle$.next(partialVehicle);
  }

  getNewVehicleObservable(): Observable<any> {
    return this.newVehicle$.asObservable();
  }

  loadVehicles(): Observable<VehicleInterface[]> {
    const url = 'http://localhost:5280/api/vehicle/GetAllVehicles';
    console.log(this.http.get<Array<VehicleInterface>>(url).pipe(retry(1)));
    return this.http.get<Array<VehicleInterface>>(url).pipe(retry(1))
  }
}
