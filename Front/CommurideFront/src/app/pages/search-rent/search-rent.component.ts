import { Component } from '@angular/core';
import { SkeletonComponent } from '../../components/skeleton/skeleton.component';
import { VehicleComponent } from '../../components/vehicle/vehicle.component';
import { InputComponent } from '../../components/form/input/input.component';
import { ButtonComponent } from '../../components/button/button.component';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-search-rent',
  standalone: true,
  imports: [
    SkeletonComponent,
    VehicleComponent,
    InputComponent,
    ButtonComponent,
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './search-rent.component.html',
  styleUrl: './search-rent.component.scss'
})
export class searchRentComponent {
  searchForm?: FormGroup = this.formBuilder.group({
    search: '',
    date: ''
  });

  constructor(private formBuilder: FormBuilder) {}
}
