import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthPageComponent } from './pages/auth-page/auth-page.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AuthPageComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'CommurideFront';
}
