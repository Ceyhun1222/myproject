import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapObstacleComponent } from './map-obstacle.component';

describe('MapObstacleComponent', () => {
  let component: MapObstacleComponent;
  let fixture: ComponentFixture<MapObstacleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapObstacleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapObstacleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
