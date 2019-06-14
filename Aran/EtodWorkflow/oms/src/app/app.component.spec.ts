import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { Component, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { Router } from '@angular/router';

@Component({selector: 'app-header', template: ''})
class HeaderComponent {}

@Component({selector: 'app-sidebar', template: ''})
class SidebarComponent { }

@Component({selector: 'router-outlet', template: ''})
class RouterOutletStubComponent {}


describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent, 
        HeaderComponent,
        SidebarComponent,
        RouterOutletStubComponent
      ],
      providers: [Router],
      schemas:[ CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA ] 
    }).compileComponents();
  }));
  it('should create the app', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
});
