import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

const USER_KEY = 'auth-user';
const TOKEN_KEY = 'auth-token';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  /**
   * clear all localstorage
   */
  clear(): void {
    window.sessionStorage.clear();
  }

  /**
   * saves the user to localstorage
   * @param user user object to save
   */
  public saveUser(user: any, token?: string): void {
    window.sessionStorage.removeItem(USER_KEY);
    window.sessionStorage.setItem(USER_KEY, JSON.stringify(user));
    if (token) {
      window.sessionStorage.removeItem(TOKEN_KEY);
      window.sessionStorage.setItem(TOKEN_KEY, token);
    }
  }

  /**
   * saves the token to localstorage
   * @param token token to save
   */
    public saveToken(token: string): void {
      window.sessionStorage.setItem(TOKEN_KEY, token);
      
    }

  /**
   * retrieves the user from localstorage if it exists, return empty object if not found
   * @returns user object
   */
  public getUser(): any {
    const user = window.sessionStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }

    return {};
  }

  /**
   * retrieves the token from localstorage if it exists, return empty string if not found
   * @returns user object
   */
  public getToken(): any {
    const token = window.sessionStorage.getItem(TOKEN_KEY);
    return token ?? "";
  }

  /**
   * check if user is logged in
   * @returns 
   */
  public isLoggedIn(): boolean {
    const user = window.sessionStorage.getItem(USER_KEY);
    if (user) {
      return true;
    }

    return false;
  }
}