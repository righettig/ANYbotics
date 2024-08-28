import { Module } from '@nestjs/common';
import { BehavioursService } from './behaviours.service';
import { BehavioursController } from './behaviours.controller';

@Module({
  providers: [BehavioursService],
  controllers: [BehavioursController]
})
export class BehavioursModule {}
