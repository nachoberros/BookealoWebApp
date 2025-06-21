import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { MenuComponent } from "./menu/menu.component";
import { AlertService } from './services/alert.service';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  imports: [MenuComponent, RouterOutlet]
})
export class AppComponent {
  public successMessage: string | null = '';

  constructor(public router: Router, private alertService: AlertService) { }

 showMenu(): boolean {
    return !!localStorage.getItem('jwtToken');
  }

  ngOnInit() {
  this.alertService.success$.subscribe(message => {
      this.successMessage = message;
  });
}
}