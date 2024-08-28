import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { Document } from 'mongoose';

export type BotDocument = Bot & Document;
export type BotLanguage = 'csharp' | 'python' | 'typescript';

@Schema()
export class BotBehaviour {
    @Prop({ required: true, enum: ['csharp', 'python', 'typescript'] })
    language: BotLanguage;

    @Prop({ required: true })
    source: string;
}

const BotBehaviourSchema = SchemaFactory.createForClass(BotBehaviour);

@Schema()
export class Bot {
    @Prop({ required: true })
    botId: string;

    @Prop({ required: true })
    userId: string;

    @Prop({ type: BotBehaviourSchema, required: true })
    behaviour: BotBehaviour;
}

export const BotSchema = SchemaFactory.createForClass(Bot);
