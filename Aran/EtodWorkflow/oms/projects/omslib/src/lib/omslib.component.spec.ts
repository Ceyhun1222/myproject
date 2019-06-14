import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OmslibComponent } from './omslib.component';

describe('OmslibComponent', () => {
  let component: OmslibComponent;
  let fixture: ComponentFixture<OmslibComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OmslibComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OmslibComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
