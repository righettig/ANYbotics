import { Controller, Get } from '@nestjs/common';

@Controller('bots')
export class BotsController {
    @Get()
    findAll(): string {
        return 'This action returns all bots';
    }
}
