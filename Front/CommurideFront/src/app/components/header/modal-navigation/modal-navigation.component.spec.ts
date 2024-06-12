import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalNavigationComponent } from './modal-navigation.component';

describe('ModalNavigationComponent', () => {
  let component: ModalNavigationComponent;
  let fixture: ComponentFixture<ModalNavigationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalNavigationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalNavigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
