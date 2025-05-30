import { Component, EventEmitter, Output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tennis-booking-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tennis-booking-search.component.html',
  styleUrls: ['./tennis-booking-search.component.css']
})
export class TennisBookingSearchComponent {
  searchForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.searchForm = this.fb.group({
      date: [new Date().toISOString().split('T')[0]]
    });
  }

  date: string = new Date().toISOString().split('T')[0];

  @Output() search = new EventEmitter<string>();

  onSearch() {
    this.search.emit(this.date);
  }
}