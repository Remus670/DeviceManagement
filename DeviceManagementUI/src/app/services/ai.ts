import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AiService {
  private apiUrl = 'http://localhost:5219/api/Ai';

  constructor(private http: HttpClient) {}

  generateDescription(device: {
    name: string;
    manufacturer: string;
    type: string;
    operatingSystem: string;
    ramAmount: string;
    processor: string;
  }): Observable<{ description: string }> {
    return this.http.post<{ description: string }>(
      `${this.apiUrl}/generate-description`,
      device
    );
  }
}