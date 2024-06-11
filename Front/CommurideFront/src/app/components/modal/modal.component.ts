import { InputComponent } from "../form/input/input.component";
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-modal',
    standalone: true,
    templateUrl: './modal.component.html',
    styleUrl: './modal.component.scss',
    imports: [InputComponent]
})
export class ModalComponent {
  @Input() title: string = '';
  @Input() isOpen: boolean = false;
  @Output() onClose: EventEmitter<any> = new EventEmitter();
  @Output() onSave: EventEmitter<any> = new EventEmitter();
  @Output() isOpenChange: EventEmitter<boolean> = new EventEmitter();

  closeModal() {
    this.isOpen = false;
    this.isOpenChange.emit(false);
    this.onClose.emit();
  }

  save() {
    this.onSave.emit();
  }

}
