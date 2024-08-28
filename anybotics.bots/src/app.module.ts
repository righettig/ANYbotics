import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { BotsModule } from './bots/bots.module';

@Module({
  imports: [
    MongooseModule.forRoot('mongodb://localhost:27017/anybotics-bots'), 
    BotsModule,
  ],
})
export class AppModule { }
