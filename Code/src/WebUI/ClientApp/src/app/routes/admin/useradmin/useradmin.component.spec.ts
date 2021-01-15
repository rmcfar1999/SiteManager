import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUseradminComponent } from './useradmin.component';

describe('UseradminComponent', () => {
  let component: AdminUseradminComponent;
  let fixture: ComponentFixture<AdminUseradminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminUseradminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUseradminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
