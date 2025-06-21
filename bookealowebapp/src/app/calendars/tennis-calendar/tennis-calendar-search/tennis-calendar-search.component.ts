import { Component, EventEmitter, Output, OnInit, Input } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Calendar } from '../../calendars.model';

@Component({
  selector: 'app-tennis-calendar-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tennis-calendar-search.component.html',
  styleUrls: ['./tennis-calendar-search.component.css']
})
export class TennisCalendarSearchComponent implements OnInit {
  @Input() calendar: Calendar | null = null;

  searchForm: FormGroup;
  date: string = new Date().toLocaleDateString('sv-SE');

  constructor(private fb: FormBuilder) {
    this.searchForm = this.fb.group({
      date: [this.formatDate(new Date())]
    });
  }

  @Output() search = new EventEmitter<string>();

  ngOnInit(): void {
    this.onSearch();
  }

  onSearch() {
    const selectedDate = this.searchForm.value.date;
    this.search.emit(selectedDate);
  }

  private formatDate(date: Date): string {
    return date.toLocaleDateString('sv-SE');
  }

  formatDateForInput(date: string | Date): string {
    const d = new Date(date);
    return d.toISOString().split('T')[0]; // returns YYYY-MM-DD
  }

  get minDate(): string {
    const today = new Date();
    const startDate = this.calendar?.startDate ? new Date(this.calendar.startDate) : null;

    if (startDate && startDate > today) {
      return this.formatDateForInput(startDate);
    }

    return this.formatDateForInput(today);
  }

  get maxDate(): string | null {
    return this.calendar?.endDate ? this.formatDateForInput(this.calendar.endDate) : null;
  }
}
