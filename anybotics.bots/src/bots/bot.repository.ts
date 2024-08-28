import { Injectable } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';
import { Bot, BotDocument } from './bot.schema';
import { IBotRepository } from './bot-repository.interface';

@Injectable()
export class BotRepository implements IBotRepository {
  constructor(@InjectModel(Bot.name) private botModel: Model<BotDocument>) {}

  async create(bot: Bot): Promise<Bot> {
    const createdBot = new this.botModel(bot);
    return createdBot.save();
  }

  async findAll(): Promise<Bot[]> {
    return this.botModel.find().exec();
  }

  async findOne(botId: string): Promise<Bot | null> {
    return this.botModel.findOne({ botId }).exec();
  }
}
