import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { StorageService } from '../../services/storage.service';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  constructor(
    private authService: AuthService, 
    private storageService: StorageService
  ) {
    
  }

  logout() {
    this.authService.logout().subscribe({
      next: (e) => {
        this.storageService.clear()
      },
      error: (e) => {
        console.log('error logout');
        console.log(e);
      }
    })
  }
}
