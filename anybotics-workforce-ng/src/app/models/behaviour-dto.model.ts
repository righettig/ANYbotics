export type BotLanguage = 'csharp' | 'python' | 'typescript';

export interface BehaviourSnippet {
    version: string;
    language: BotLanguage;
    code: string;
}