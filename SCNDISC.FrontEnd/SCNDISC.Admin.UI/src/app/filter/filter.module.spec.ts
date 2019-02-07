import { FilterModule } from './filter.module';

describe('FilterModule', () => {
  let filterModule: FilterModule;

  beforeEach(() => {
    filterModule = new FilterModule();
  });

  it('should create an instance', () => {
    expect(filterModule).toBeTruthy();
  });
});
