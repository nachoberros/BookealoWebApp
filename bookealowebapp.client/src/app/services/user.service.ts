import { Injectable } from "@angular/core";
import { User } from "./auth.service";

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private currentUser: User | null = null;
  private storageKey = 'currentUser';

  constructor() {
    const storedUser = localStorage.getItem(this.storageKey);
    if (storedUser) {
      this.currentUser = JSON.parse(storedUser);
    }
  }

  setCurrentUser(user: User): void {
    this.currentUser = user;
    localStorage.setItem(this.storageKey, JSON.stringify(user));
  }

  getCurrentUser(): User | null {
    return this.currentUser;
  }

  clearCurrentUser(): void {
    this.currentUser = null;
    localStorage.removeItem(this.storageKey);
  }
}