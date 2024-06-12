import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { StorageService } from '../../../services/storage/storage.service';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-modal-navigation',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './modal-navigation.component.html',
  styleUrl: './modal-navigation.component.scss'
})
export class ModalNavigationComponent {

  /**
   *
   */
  constructor(
    private authService: AuthService, 
    private storageService: StorageService,
  ) {}
  
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
