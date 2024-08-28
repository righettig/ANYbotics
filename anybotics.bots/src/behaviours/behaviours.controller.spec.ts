import { Test, TestingModule } from '@nestjs/testing';
import { BehavioursController } from './behaviours.controller';

describe('BehavioursController', () => {
  let controller: BehavioursController;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [BehavioursController],
    }).compile();

    controller = module.get<BehavioursController>(BehavioursController);
  });

  it('should be defined', () => {
    expect(controller).toBeDefined();
  });
});
