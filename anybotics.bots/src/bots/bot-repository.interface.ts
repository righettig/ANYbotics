import { Bot } from './bot.schema';

export interface IBotRepository {
    create(bot: Bot): Promise<Bot>;
    findAll(): Promise<Bot[]>;
    findOne(botId: string): Promise<Bot | null>;
}
