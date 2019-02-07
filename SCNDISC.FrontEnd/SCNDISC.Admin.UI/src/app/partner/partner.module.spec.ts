import { PartnerModule } from './partner.module';

describe('PartnerModule', () => {
  let partnerModule: PartnerModule;

  beforeEach(() => {
    partnerModule = new PartnerModule();
  });

  it('should create an instance', () => {
    expect(partnerModule).toBeTruthy();
  });
});
