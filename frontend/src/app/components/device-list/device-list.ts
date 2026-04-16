import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DeviceService } from '../../services/device';
import { Device } from '../../models/device.model';

@Component({
  selector: 'app-device-list',
  imports: [CommonModule],
  templateUrl: './device-list.html',
  styleUrl: './device-list.css'
})
export class DeviceList implements OnInit {
  devices: Device[] = [];
  loading = true;

  constructor(private deviceService: DeviceService, private router: Router) {}

  ngOnInit(): void {
    this.loadDevices();
  }

  loadDevices(): void {
    this.loading = true;
    this.deviceService.getAll().subscribe({
      next: (data) => {
        this.devices = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading devices', err);
        this.loading = false;
      }
    });
  }

  goToCreate(): void {
    this.router.navigate(['/devices/new']);
  }

  viewDevice(id: number): void {
    this.router.navigate(['/devices', id]);
  }

  editDevice(id: number): void {
    this.router.navigate(['/devices', id, 'edit']);
  }

  deleteDevice(id: number): void {
    if (confirm('Are you sure you want to delete this device?')) {
      this.deviceService.delete(id).subscribe({
        next: () => this.loadDevices(),
        error: (err) => console.error('Error deleting device', err)
      });
    }
  }
}