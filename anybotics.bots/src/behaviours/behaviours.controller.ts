import { Controller, Get } from '@nestjs/common';
import { BotLanguage } from 'src/bots/bot.schema';

export interface BehaviourSnippet {
    version: string;
    language: BotLanguage;
    code: string;
}

const predefined: BehaviourSnippet[] = [
    {
        version: 'v1',
        language: 'csharp',
        code: `
            public class Bot
            {
                public ICommand Update(IWorldState worldState)
                {
                    // Return an appropriate ICommand instance
                }
            }
        `,
    },
    {
        version: 'v1',
        language: 'python',
        code: `
            class Bot:
                def update(self, world_state: IWorldState) -> ICommand:
                    # Return an appropriate ICommand instance
        `,
    },
    {
        version: 'v1',
        language: 'ts',
        code: `
            class Bot {
                update(worldState: IWorldState): ICommand {
                    // Return an appropriate ICommand instance
                }
            }
        `,
    }
];

@Controller('behaviours')
export class BehavioursController {
    @Get()
    async findAll(): Promise<BehaviourSnippet[]> {
        return predefined;
    }
}
