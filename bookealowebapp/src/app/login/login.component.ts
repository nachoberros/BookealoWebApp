import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import {AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    loginForm: FormGroup;

    @Output() login = new EventEmitter<{ username: string; password: string }>();
    constructor(
        private fb: FormBuilder,
        private http: HttpClient,
        private router: Router,
        private authService: AuthService,
        private userService: UserService
    ) {
        this.loginForm = this.fb.group({
            email: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    onSubmit(): void {
        if (this.loginForm.valid) {
            this.http.post('/api/auth/login', this.loginForm.value).subscribe({
                next: (response: any) => {
                    this.authService.login(response.token, response.user);
                    this.userService.setCurrentUser(response.user);
                    this.router.navigate(['/home']);
                },
                error: (err) => {
                    console.error('Login failed', err);
                }
            });
        }
    }
}
