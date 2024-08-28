import { Test, TestingModule } from '@nestjs/testing';
import { BehavioursService } from './behaviours.service';

describe('BehavioursService', () => {
  let service: BehavioursService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [BehavioursService],
    }).compile();

    service = module.get<BehavioursService>(BehavioursService);
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });
});
