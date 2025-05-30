import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService, User } from '../services/user.service';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent {
  @Input() currentUser: User | undefined;

  constructor(private userService: UserService) {}

ngOnInit() {
  const user = {id: 4, name: "Charlie"};
  this.userService.setCurrentUser(user);
  this.currentUser = user;
}

  menuItems = [
    { label: 'Home', route: '/' },
    { label: 'About', route: '/about' },
    {label: 'Tennis', route: '/tennis'}
  ];
}