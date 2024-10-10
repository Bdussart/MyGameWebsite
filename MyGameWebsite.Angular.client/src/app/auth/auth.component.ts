import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent 
  implements OnInit {
    loginForm!: FormGroup;
    loading = false;
    submitted = false;
    error = '';
    jwt = '';
    
    constructor(
      private formBuilder: FormBuilder,
      private router: Router,
      private authService: AuthService
    ) {}
  
    ngOnInit() {
      this.loginForm = this.formBuilder.group({
        username: ['', Validators.required],
        password: ['', Validators.required]
      });
    }
  
    get f() { return this.loginForm.controls; }
  
    onSubmit() {
      this.submitted = true;
  
      // stop here if form is invalid
      if (this.loginForm.invalid) {
        return;
      }
  
      this.loading = true;
      this.authService.login(this.f['username'].value, this.f['password'].value)
        .subscribe({
          next: () => {
            // this.router.navigate(['/']);

            const userdata  = localStorage.getItem('currentUser');
            this.jwt = userdata ? JSON.parse(userdata)['Token'] : '';

          },
          error: error => {
            this.error = error.value;
            this.loading = false;
             throwError(() => this.error);
          }
        });
    }
  }

