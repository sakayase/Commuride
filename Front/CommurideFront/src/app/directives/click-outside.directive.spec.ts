import { ClickOutsideDirective } from './click-outside.directive';

describe('ClickOutsideDirective', () => {
  it('should create an instance', () => {
    let elRefMock = {
      nativeElement: document.createElement('div')
    };
    
    const directive = new ClickOutsideDirective(elRefMock);
    expect(directive).toBeTruthy();
  });
});
