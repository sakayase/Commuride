import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-skeleton',
  standalone: true,
  imports: [ CommonModule],
  templateUrl: './skeleton.component.html',
  styleUrl: './skeleton.component.scss'
})
export class SkeletonComponent {
  isMobile: boolean = false;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    if (isPlatformBrowser(this.platformId)) {
      const userAgent = window.navigator.userAgent;
      this.isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(userAgent);
    }
  }
}