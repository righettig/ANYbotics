import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { BotsModule } from './bots/bots.module';
import { BehavioursModule } from './behaviours/behaviours.module';

@Module({
  imports: [
    MongooseModule.forRoot(process.env.MONGO_URL!),
    BotsModule, BehavioursModule,
  ],
})
export class AppModule { }
