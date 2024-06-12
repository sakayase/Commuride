import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Router, RouterLink } from '@angular/router';
import { StorageService } from '../../services/storage/storage.service';
import { ButtonComponent } from '../../components/button/button.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [ButtonComponent, RouterLink],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  constructor(
    private authService: AuthService, 
    private storageService: StorageService,
    private router: Router
  ) {
    
  }

  logout() {
    this.authService.logout().subscribe({
      next: (e) => {
        this.storageService.clear();
        window.location.reload();
      },
      error: (e) => {
        console.log('error logout');
        console.log(e);
      }
    })
  }
}
