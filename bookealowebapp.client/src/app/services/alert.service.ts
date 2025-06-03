import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  private successSubject = new BehaviorSubject<string | null>(null);
  success$ = this.successSubject.asObservable();

  showSuccess(message: string, duration = 3000) {
    this.successSubject.next(message);
    setTimeout(() => this.clearSuccess(), duration);
  }

  clearSuccess() {
    this.successSubject.next(null);
  }
}
