import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DeviceService } from '../../services/device';
import { Device } from '../../models/device.model';

@Component({
  selector: 'app-device-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './device-list.html',
  styleUrl: './device-list.css'
})
export class DeviceList implements OnInit {
  devices: Device[] = [];
  loading = true;
  showDeleteModal = false;
  deviceToDeleteId: number | null = null;
  deviceToDeleteName = '';
  searchQuery = '';
  searching = false;
  hasSearched = false;

  constructor(
    private deviceService: DeviceService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.hasSearched = false;
    this.searchQuery = '';
    this.loadDevices();
  }

  loadDevices(): void {
    this.loading = true;
    this.deviceService.getAll().subscribe({
      next: (data) => {
        this.devices = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error:', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  search(): void {
    if (!this.searchQuery.trim()) {
      this.hasSearched = false;
      this.loadDevices();
      return;
    }

    this.searching = true;
    this.hasSearched = true;
    this.deviceService.search(this.searchQuery).subscribe({
      next: (data) => {
        this.devices = data;
        this.searching = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error:', err);
        this.searching = false;
        this.cdr.detectChanges();
      }
    });
  }

  clearSearch(): void {
    this.searchQuery = '';
    this.hasSearched = false;
    this.loadDevices();
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

  confirmDelete(id: number, name: string): void {
    this.deviceToDeleteId = id;
    this.deviceToDeleteName = name;
    this.showDeleteModal = true;
    this.cdr.detectChanges();
  }

  cancelDelete(): void {
    this.showDeleteModal = false;
    this.deviceToDeleteId = null;
    this.deviceToDeleteName = '';
    this.cdr.detectChanges();
  }

  executeDelete(): void {
    if (this.deviceToDeleteId) {
      this.deviceService.delete(this.deviceToDeleteId).subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.ngOnInit();
        },
        error: (err) => console.error('Error deleting device', err)
      });
    }
  }
}