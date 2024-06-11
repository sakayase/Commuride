import { Component } from '@angular/core';
import vehicleInterface, { CategoryVehicle, MotorizationVehicle, StatusVehicle } from '../../../interfaces/vehicleInterface';
import { VehicleService } from '../../../services/vehicle/vehicle.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vehicle',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './vehicle.component.html',
  styleUrl: './vehicle.component.scss'
})

export class VehicleComponent {
  title = 'vehicle'
  vehicles: vehicleInterface[] = [];

  constructor(private vehicleService: VehicleService) {}

  ngOnInit(): void {
    this.vehicleService.loadVehicles().subscribe({
      next: (Vehicles: vehicleInterface[]) => {
        this.vehicles = Vehicles;
        console.log(this.vehicles)
      },
      error: (error) => {
        console.log(error);
      },
    });

    this.vehicleService.getNewVehicleObservable().subscribe({
      next: (partialVehicle: Omit<vehicleInterface, 'id'>) => {
        console.log(partialVehicle);
      }
    });
  }
  getCategoryName(category: CategoryVehicle): string {
    return this.vehicleService.getCategoryName(category);
  }

  getMotorizationName(motorization: MotorizationVehicle): string {
    return this.vehicleService.getMotorizationName(motorization);
  }

  getStatusName(status: StatusVehicle): string {
    return this.vehicleService.getStatusName(status);
  }
}
