import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { BotService } from './bots.service';
import { BotController } from './bots.controller';
import { Bot, BotSchema } from './bot.schema';
import { BotRepository } from './bot.repository';

@Module({
  imports: [MongooseModule.forFeature([{ name: Bot.name, schema: BotSchema }])],
  controllers: [BotController],
  providers: [
    BotService,
    {
      provide: 'IBotRepository',
      useClass: BotRepository, // or MemoryBotRepository
    },
  ],
})
export class BotsModule { }
