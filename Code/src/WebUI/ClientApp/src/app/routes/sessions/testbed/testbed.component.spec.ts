import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SessionsTestbedComponent } from './testbed.component';

describe('TestbedComponent', () => {
  let component: SessionsTestbedComponent;
  let fixture: ComponentFixture<SessionsTestbedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SessionsTestbedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SessionsTestbedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
