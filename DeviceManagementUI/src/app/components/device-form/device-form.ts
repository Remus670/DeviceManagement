import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DeviceService } from '../../services/device';
import { AiService } from '../../services/ai';
import { Device } from '../../models/device.model';

@Component({
  selector: 'app-device-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './device-form.html',
  styleUrl: './device-form.css'
})
export class DeviceForm implements OnInit {
  isEditMode = false;
  deviceId: number | null = null;
  errorMessage = '';
  successMessage = '';
  generatingDescription = false;

  device: Device = {
    id: 0,
    name: '',
    manufacturer: '',
    type: 'Phone',
    operatingSystem: '',
    osVersion: '',
    processor: '',
    ramAmount: '',
    description: ''
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private deviceService: DeviceService,
    private aiService: AiService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.deviceId = Number(id);
      this.deviceService.getById(this.deviceId).subscribe({
        next: (data) => {
          this.device = data;
          this.cdr.detectChanges();
        },
        error: (err) => console.error('Error:', err)
      });
    }
  }

  validateForm(): boolean {
    if (!this.device.name.trim()) { this.errorMessage = 'Name is required'; return false; }
    if (!this.device.manufacturer.trim()) { this.errorMessage = 'Manufacturer is required'; return false; }
    if (!this.device.type.trim()) { this.errorMessage = 'Type is required'; return false; }
    if (!this.device.operatingSystem.trim()) { this.errorMessage = 'Operating System is required'; return false; }
    if (!this.device.osVersion.trim()) { this.errorMessage = 'OS Version is required'; return false; }
    if (!this.device.processor.trim()) { this.errorMessage = 'Processor is required'; return false; }
    if (!this.device.ramAmount.trim()) { this.errorMessage = 'RAM Amount is required'; return false; }
    return true;
  }

  generateDescription(): void {
    if (!this.device.name || !this.device.manufacturer || !this.device.processor || !this.device.ramAmount) {
      this.errorMessage = 'Please fill Name, Manufacturer, Processor and RAM before generating description';
      return;
    }

    this.generatingDescription = true;
    this.errorMessage = '';

    this.aiService.generateDescription({
      name: this.device.name,
      manufacturer: this.device.manufacturer,
      type: this.device.type,
      operatingSystem: this.device.operatingSystem,
      ramAmount: this.device.ramAmount,
      processor: this.device.processor
    }).subscribe({
      next: (res) => {
        this.device.description = res.description;
        this.generatingDescription = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.errorMessage = 'Error generating description';
        this.generatingDescription = false;
        this.cdr.detectChanges();
      }
    });
  }

  onSubmit(): void {
    this.errorMessage = '';
    if (!this.validateForm()) return;

    if (this.isEditMode && this.deviceId) {
      this.deviceService.update(this.deviceId, this.device).subscribe({
        next: () => {
          this.successMessage = 'Device updated successfully!';
          setTimeout(() => this.router.navigate(['/devices']), 1000);
        },
        error: (err) => {
          this.errorMessage = 'Error updating device';
          console.error(err);
        }
      });
    } else {
      this.deviceService.getAll().subscribe({
        next: (devices) => {
          const exists = devices.some(d =>
            d.name.toLowerCase() === this.device.name.toLowerCase().trim()
          );
          if (exists) {
            this.errorMessage = 'A device with this name already exists!';
            this.cdr.detectChanges();
            return;
          }
          this.deviceService.create(this.device).subscribe({
            next: () => {
              this.successMessage = 'Device created successfully!';
              setTimeout(() => this.router.navigate(['/devices']), 1000);
            },
            error: (err) => {
              this.errorMessage = 'Error creating device';
              console.error(err);
            }
          });
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/devices']);
  }
}