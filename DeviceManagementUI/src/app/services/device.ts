import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Device } from '../models/device.model';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  private apiUrl = 'http://localhost:5219/api/Devices';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Device[]> {
    return this.http.get<Device[]>(this.apiUrl);
  }

  getById(id: number): Observable<Device> {
    return this.http.get<Device>(`${this.apiUrl}/${id}`);
  }

  create(device: Device): Observable<Device> {
    return this.http.post<Device>(this.apiUrl, device);
  }

  update(id: number, device: Device): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, device);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  assign(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/assign`, {});
  }

  unassign(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/unassign`, {});
  }

  search(query: string): Observable<Device[]> {
  return this.http.get<Device[]>(`${this.apiUrl}/search?q=${encodeURIComponent(query)}`);
}

}