import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DeviceService } from '../../services/device';
import { AuthService } from '../../services/auth';
import { Device } from '../../models/device.model';

@Component({
  selector: 'app-device-detail',
  imports: [CommonModule],
  templateUrl: './device-detail.html',
  styleUrl: './device-detail.css'
})
export class DeviceDetail implements OnInit {
  device: Device | null = null;
  loading = true;
  message = '';
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private deviceService: DeviceService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadDevice(id);
  }

  loadDevice(id: number): void {
    this.deviceService.getById(id).subscribe({
      next: (data) => {
        this.device = data;
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

  get isUnassigned(): boolean {
    return !this.device?.userId;
  }

  get currentUserEmail(): string {
    return this.authService.getEmail() || '';
  }

  get isAssignedToMe(): boolean {
    return !!this.device?.user && 
           this.device.user.name === this.currentUserEmail;
  }

  assign(): void {
    if (!this.device) return;
    this.deviceService.assign(this.device.id).subscribe({
      next: () => {
        this.message = 'Device assigned successfully!';
        this.errorMessage = '';
        this.loadDevice(this.device!.id);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Error assigning device';
        this.cdr.detectChanges();
      }
    });
  }

  unassign(): void {
    if (!this.device) return;
    this.deviceService.unassign(this.device.id).subscribe({
      next: () => {
        this.message = 'Device unassigned successfully!';
        this.errorMessage = '';
        this.loadDevice(this.device!.id);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Error unassigning device';
        this.cdr.detectChanges();
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/devices']);
  }

  editDevice(): void {
    this.router.navigate(['/devices', this.device?.id, 'edit']);
  }
}