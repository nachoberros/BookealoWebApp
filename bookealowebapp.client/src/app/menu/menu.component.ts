import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService, User } from '../services/auth.service';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  currentUser: User | null = null;
  showUserMenu = false;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
  }

  logout() {
    this.authService.logout();
    this.showUserMenu = false;
  }

  toggleMenu() {
    this.showUserMenu = !this.showUserMenu;
  }

  menuItems = [
    { label: 'Home', route: '/home' },
    { label: 'About', route: '/about' },
    { label: 'Tennis', route: '/tennis' }
  ];
}
