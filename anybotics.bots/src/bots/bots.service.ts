import { Inject, Injectable } from '@nestjs/common';
import { Bot } from './bot.schema';
import { IBotRepository } from './bot-repository.interface';

@Injectable()
export class BotService implements IBotRepository {
    constructor(@Inject('IBotRepository') private readonly botRepository: IBotRepository) { }

    async create(botData: Bot): Promise<Bot> {
        return this.botRepository.create(botData);
    }

    async findAll(): Promise<Bot[]> {
        return this.botRepository.findAll();
    }

    async findOne(botId: string): Promise<Bot | null> {
        return this.botRepository.findOne(botId);
    }

    async validate(botId: string): Promise<boolean> {
        const bot = await this.botRepository.findOne(botId);
        if (!bot) return false;

        if (bot.behaviour.language && bot.behaviour.source) {
            // TODO: check that 'source' is a valid instance of a program written in 'language'.
            return true;
        }
        return false;
    }
}
