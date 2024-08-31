import { Module } from '@nestjs/common';
import { EventsController } from './events.controller';
import { EventsService } from './events.service';
import { ConfigService } from './config.service';

@Module({
  controllers: [EventsController],
  providers: [EventsService, ConfigService]
})
export class EventsModule {}
