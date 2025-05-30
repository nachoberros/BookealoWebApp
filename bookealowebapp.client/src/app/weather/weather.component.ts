import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-weather',
  standalone: true,
  imports: [CommonModule], // Add more modules here if needed
  templateUrl: './weather.component.html',
})
export class WeatherComponent implements OnInit {
  forecasts: any[] | null = null;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any[]>('/weatherforecast').subscribe(data => {
      this.forecasts = data;
    });
  }
}