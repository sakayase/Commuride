import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';
import { InputComponent } from '../../components/form/input/input.component';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { StorageService } from '../../services/storage/storage.service';
import { ButtonComponent } from '../../components/button/button.component';

@Component({
  selector: 'app-auth-page',
  standalone: true,
  imports: [
    CommonModule,
    InputComponent,
    FormsModule,
    ReactiveFormsModule,
    ButtonComponent
  ],
  templateUrl: './auth-page.component.html',
  styleUrl: './auth-page.component.scss'
})
export class AuthPageComponent {
  user?: object;
  error: boolean = false;
  loginForm?: FormGroup = this.formBuilder.group({
    username: '',
    password: '',
  });

  constructor(
    private authService: AuthService, 
    private storageService: StorageService, 
    private formBuilder: FormBuilder,
    private router: Router,
  ) {}

  submit() {
    this.login(this.loginForm!.controls['username'].value, this.loginForm!.controls['password'].value)
  }

  login(username: string, password: string) {
    this.error = false;
    this.authService.login({username: username, password: password}).subscribe({
      next: (e) => {
        const token: string = e.split(' ')[1];
        this.storageService.saveToken(token);
        this.getLoggedUser();
      },
      error: (e) => {
        this.error = true;
        console.log('error login')
        console.log(e);
      }
    });
  }

  getLoggedUser() {
    this.authService.getUserObject(this.storageService.getToken()).subscribe({
      next: (e) => {
        this.storageService.saveUser(e);
        this.user = this.storageService.getUser();
        this.router.navigate(['']);
      },
      error: (e) => {
        this.error = true;
        console.log('error get user');
        console.log(e);
      }
    })
  }

  logout() {
    this.authService.logout().subscribe({
      next: (e) => {
        this.storageService.clear()
        this.user = undefined;
      },
      error: (e) => {
        console.log('error logout');
        console.log(e);
      }
    })
  }
}
