import { TestBed, inject } from '@angular/core/testing';

import { OmslibService } from './omslib.service';

describe('OmslibService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OmslibService]
    });
  });

  it('should be created', inject([OmslibService], (service: OmslibService) => {
    expect(service).toBeTruthy();
  }));
});
