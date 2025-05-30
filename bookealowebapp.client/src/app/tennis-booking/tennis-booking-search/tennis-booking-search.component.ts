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
      date: [this.formatDate(new Date())]
    });
  }

  date: string = new Date().toISOString().split('T')[0];

  @Output() search = new EventEmitter<string>();

  onSearch() {
    const selectedDate = this.searchForm.value.date;
    this.search.emit(selectedDate);
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }
}