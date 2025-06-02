import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Role, User } from '../users.model';

@Component({
    selector: 'app-edit-user',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCheckboxModule, MatButtonModule, MatProgressSpinnerModule],
    templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {
    userId!: string;
    user: User = {
        id: 0, name: '',
        email: '',
        role: Role.Guest
    };
    loading: boolean = false;

    private route = inject(ActivatedRoute);
    private http = inject(HttpClient);
    private router = inject(Router);

    ngOnInit(): void {
        const idParam = this.route.snapshot.paramMap.get('userId');

        if (idParam) {
            this.userId = idParam;
            this.loading = true;
            this.http.get<User>(`/api/user/${this.userId}`).subscribe({
                next: data => {
                    this.user = data;
                    this.loading = false;
                },
                error: err => {
                    console.error('Failed to load user', err);
                    this.loading = false;
                }
            });
        }
    }

    saveUser() {
        const request = this.userId
            ? this.http.put(`/api/user`, this.user)
            : this.http.post(`/api/user`, this.user);

        request.subscribe({
            next: () => {
                alert(`User ${this.userId ? 'updated' : 'created'} successfully!`);
                this.router.navigate(['/users']);
            },
            error: err => {
                console.error('Failed to save user', err);
            }
        });
    }

    cancel() {
        this.router.navigate(['/users']);
    }
}
