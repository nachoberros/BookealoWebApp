import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { MenuComponent } from "./menu/menu.component";

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  imports: [MenuComponent, RouterOutlet]
})
export class AppComponent {
  constructor(public router: Router) { }

 showMenu(): boolean {
    return !!localStorage.getItem('jwtToken');
  }
}