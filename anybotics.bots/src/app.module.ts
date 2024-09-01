import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { BotsModule } from './bots/bots.module';
import { BehavioursModule } from './behaviours/behaviours.module';
import { ThrottlerGuard, ThrottlerModule } from '@nestjs/throttler';
import { APP_GUARD } from '@nestjs/core';

@Module({
  imports: [
    MongooseModule.forRoot(process.env.MONGO_URL!),
    ThrottlerModule.forRoot([{
      ttl: 60 * 1000,
      limit: 10,
    }]),
    BotsModule, 
    BehavioursModule,
  ],
  providers: [
    {
      provide: APP_GUARD,
      useClass: ThrottlerGuard
    }    
  ]
})
export class AppModule { }
