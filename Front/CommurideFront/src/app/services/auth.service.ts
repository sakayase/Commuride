import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import LoginInterface from '../../interfaces/loginInterface';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  url:string = "https://localhost:7052/api/Auth"; 

  constructor(private client: HttpClient) { }

  login(login: LoginInterface): Observable<string> {
    const options: object = { responseType: "text" };
    return this.client.post<string>(`${this.url}/Login/`, login, options)
  }

  logout(): Observable<any> {
    const options: object = { responseType: "text", observe: 'response' as 'response' };
    return this.client.get(`${this.url}/Logout`, options)
  }

  getUserObject(token:string): Observable<any> {
    const options: object = {};
    return this.client.get(`${this.url}/GetConnectedUser/`, options);
  }
}


//.AspNetCore.Identity.Application=CfDJ8JawmfcMSKdPjdYsxxc6diECaiX3dpykB-VaQA30h_ZBwi42i7eaQsyN3PNeSmmC_6_9BIejLJTZJ_GLmTW7lSLutinl1Oxtw0DQ9S0NOiN2YBFfX4PKCbbyyIEi1pcK0wi0WJ6d_xMtNMBPyEgSWN62dVes1zGCTAgNKZhUkg7R8fuwWdXLY0FRYsXok7adV8MfJDrZcPZYAeVZi0KyOaowjkoANclIhSkSzp21NTPkbNH9T1CYDEj4axE-24aA-nAHiR5OSCD2hx1AGxaS58bR9RDYZegXRkSpRgAV6zROWMemXFAgDbVdOkJ9TCXO_dV7PHtNdOKAAhy57iB_Ps5nZkfptYirXH9yg__87C3jCFsUQi37grj-GUy70KEke5NWZNPuGUgktmo7XLGnlLH-EC1VU-VOBttXRcUrk5fRWqBvyIOmBtUwjV0_7P2qiSKncbLR0maavt-FNVjVTvh63rBDcfXRljI64BOIiAaN9-mtGGhXnG8etvbebgwxb-yN4DBkiBTlq_WSTmIlIJPPW557eGCu4dRzQyMFRRREl0vymiAqMvZ9tEgOZUQ9E_l-y41Hjop2Y5HXK0VNL53qbP9uTA_pGOWvQSnZDwbn; path=/; secure; samesite=lax; httponly
