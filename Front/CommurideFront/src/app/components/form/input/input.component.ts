import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, Output } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss'
})
export class InputComponent {
  @Input() iconUrl: string | undefined;
  @Input() subject: string | undefined;
  @Input() label: string | undefined;
  @Input() formControlName!: string;
  @Input() form!: FormGroup;


  @Output() value: string | undefined;
}
