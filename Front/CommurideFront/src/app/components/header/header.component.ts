import { Component } from '@angular/core';
import { ModalNavigationComponent } from './modal-navigation/modal-navigation.component';
import { ClickOutsideDirective } from '../../directives/click-outside.directive';
import { StorageService } from '../../services/storage/storage.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [ModalNavigationComponent, ClickOutsideDirective],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  showModal: boolean = false;
  buttonId: string = "open-modal-button";
  constructor(private storageService: StorageService) {
  }

  isUserLogged(): boolean {
    return this.storageService.isLoggedIn()
  }

  toggleModal() {
    this.showModal = !this.showModal;
  }
}


