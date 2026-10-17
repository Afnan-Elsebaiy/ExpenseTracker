import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiResponse } from '../models/api-response';
import { LoginRequest } from '../models/authentication/request/login-request';
import { LoginResponse } from '../models/authentication/response/login-response';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private readonly http = inject(HttpClient);

 private readonly apiUrl =
  'https://localhost:7045/api/Authentication';

  login(
    request: LoginRequest
  ): Observable<ApiResponse<LoginResponse>> {

    return this.http.post<ApiResponse<LoginResponse>>(
      `${this.apiUrl}/login`,
      request
    );
  }
}