import { Injectable } from '@nestjs/common';
import { Bot } from './bot.schema';
import { IBotRepository } from './bot-repository.interface';

@Injectable()
export class MemoryBotRepository implements IBotRepository {
    private bots: Bot[] = [];

    async create(bot: Bot): Promise<Bot> {
        this.bots.push(bot);
        return bot;
    }

    async findAll(): Promise<Bot[]> {
        return this.bots;
    }

    async findOne(botId: string): Promise<Bot | null> {
        return this.bots.find(bot => bot.botId === botId) || null;
    }
}
