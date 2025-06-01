import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { User } from '../services/auth.service';

@Component({
    selector: 'app-users',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
    users: User[] = [];
    loading: boolean = false;

    constructor(
        private http: HttpClient,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.refreshUsers();
    }

    editUser(user: User) {
        console.log('Edit', user);
        this.router.navigate(['/users/edit', user.id]);
    }

    deleteUser(user: User) {
        if (!user) return;

        if (confirm(`Are you sure you want to delete "${user.name}"?`)) {
            this.users = this.users.filter(c => c.id !== user.id);

            const params = { id: user.id };

            this.http.delete('/api/user', { params }).subscribe({
                next: () => this.refreshUsers(),
                error: err => console.error('User deletion failed', err)
            });
        }
    }

    refreshUsers() {
        this.loading = true;
        this.http.get<User[]>(`/api/user`).subscribe({
            next: data => {
                this.users = data;
                setTimeout(() => this.loading = false, 500);
            },
            error: error => {
                console.error('Failed to fetch users data', error);
                this.loading = false;
            }
        });
    }
}
