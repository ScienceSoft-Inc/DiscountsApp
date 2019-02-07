import { FeedbackModule } from './feedback.module';

describe('FeedbackModule', () => {
  let feedbackModule: FeedbackModule;

  beforeEach(() => {
    feedbackModule = new FeedbackModule();
  });

  it('should create an instance', () => {
    expect(feedbackModule).toBeTruthy();
  });
});
