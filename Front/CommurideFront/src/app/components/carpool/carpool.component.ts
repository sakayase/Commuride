import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { carpoolInterface } from '../../interfaces/carpoolInterface';
import { carpoolService } from '../../services/carpool/carpool.service';
import VehicleInterface from '../../interfaces/vehicleInterface';
import { ButtonComponent } from '../button/button.component';

@Component({
  selector: 'app-carpool',
  standalone: true,
  imports: [CommonModule, ButtonComponent],
  templateUrl: './carpool.component.html',
  styleUrl: './carpool.component.scss'
})
export class CarpoolComponent {
  title = 'carpool'
  carpools: carpoolInterface[] = [];

  constructor(private carpoolService: carpoolService) {}

  ngOnInit(): void {
    this.carpoolService.loadCarpools().subscribe({
      next: (Carpools: carpoolInterface[]) => {
        this.carpools = Carpools;
        console.log(this.carpools)
      },
      error: (error) => {
        console.log(error);
      },
    });

    this.carpoolService.getNewCarpoolObservable().subscribe({
      next: (partialcarpool: Omit<carpoolInterface, 'id'>) => {
        console.log(partialcarpool);
      }
    });
  }
}
