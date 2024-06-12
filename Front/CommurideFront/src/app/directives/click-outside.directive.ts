import { Directive, ElementRef, EventEmitter, HostListener, Input, Output } from '@angular/core';

@Directive({
  selector: '[appClickOutside]',
  standalone: true
})
export class ClickOutsideDirective {
  @Input() buttonId: string | undefined;
  @Output()
  clickOutside: EventEmitter<Event> = new EventEmitter<Event>();


  @HostListener('document:click', ['$event'])
  onClick(event: Event) {
    const clickedElement = event.target as HTMLElement;
    //Check if element clicked is the modal (elemRef is the element on which the directive is applied) 
    //and if the element id is different than the id passed in buttonId
    if ((!this.elemRef.nativeElement.contains(event.target)) && (clickedElement.id != this.buttonId)) {
      this.clickOutside.emit(event);
    }
  }

  constructor(private elemRef: ElementRef) {
  }
}
