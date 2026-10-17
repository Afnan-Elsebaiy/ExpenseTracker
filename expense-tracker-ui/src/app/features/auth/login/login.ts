import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AuthenticationService } from '../../../core/services/authentication.service';
import { LoginRequest } from '../../../core/models/authentication/request/login-request';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  private readonly authenticationService = inject(AuthenticationService);

  loginRequest: LoginRequest = {
    email: '',
    password: ''
  };

  login(): void {
    this.authenticationService
      .login(this.loginRequest)
      .subscribe({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.error(error);
        }
      });
  }
}