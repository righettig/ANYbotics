import { Controller, Get, Post, Body, Param } from '@nestjs/common';
import { BotService } from './bots.service';
import { Bot } from './bot.schema';

@Controller('bots')
export class BotController {
    constructor(private readonly botService: BotService) { }

    @Post()
    async create(@Body() botData: Bot): Promise<Bot> {
        return this.botService.create(botData);
    }

    @Get()
    async findAll(): Promise<Bot[]> {
        return this.botService.findAll();
    }

    @Get()
    async findOne(botId: string): Promise<Bot | null> {
        return this.botService.findOne(botId);
    }

    @Get('validate/:botId')
    async validate(@Param('botId') botId: string): Promise<boolean> {
        return this.botService.validate(botId);
    }
}
