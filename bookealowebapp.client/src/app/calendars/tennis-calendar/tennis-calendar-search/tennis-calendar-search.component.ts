import { Component, EventEmitter, Output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tennis-calendar-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tennis-calendar-search.component.html',
  styleUrls: ['./tennis-calendar-search.component.css']
})
export class TennisCalendarSearchComponent {
  searchForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.searchForm = this.fb.group({
      date: [this.formatDate(new Date())]
    });
  }

  date: string = new Date().toLocaleDateString('sv-SE');

  @Output() search = new EventEmitter<string>();

  onSearch() {
    const selectedDate = this.searchForm.value.date;
    this.search.emit(selectedDate);
  }

  private formatDate(date: Date): string {
    return date.toLocaleDateString('sv-SE');
  }
}