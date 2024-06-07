import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { StorageService } from '../../services/storage.service';
import { CookieService } from 'ngx-cookie-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-auth-form',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './auth-form.component.html',
  styleUrl: './auth-form.component.scss'
})
export class AuthFormComponent {
  user?:object;

  constructor(
    private authService: AuthService, 
    private storageService: StorageService, 
  ) {}

  login() {
    this.authService.login({username: "string", password: "string"}).subscribe({
      next: (e) => {
        console.log("ok login")
        console.log(e);
        this.getLoggedUser();
      },
      error: (e) => {
        console.log("error login")
        console.log(e);
      }
    });
  }

  getLoggedUser() {
    this.authService.getUserObject().subscribe({
      next: (e) => {
        console.log("ok get user");
        console.log(e);
        this.user = this.storageService.getUser();
        console.log(`user: ${JSON.stringify(this.user)}`);
      },
      error: (e) => {
        console.log("error get user");
        console.log(e);
      }
    })
  }

  logout() {
    this.authService.logout().subscribe({
      next: (e) => {
        console.log("ok logout");
        console.log(e);
        this.storageService.clean()
        this.user = undefined;
      },
      error: (e) => {
        console.log("error logout");
        console.log(e);
      }
    })
  }
}
