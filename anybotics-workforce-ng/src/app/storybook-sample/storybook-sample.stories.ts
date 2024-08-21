
import type { Meta, StoryObj } from '@storybook/angular';

import { argsToTemplate } from '@storybook/angular';
import { action } from '@storybook/addon-actions';

import StorybookSampleComponent from './storybook-sample.component';

export const actionsData = {
    onPinTask: action('onPinTask'),
    onArchiveTask: action('onArchiveTask'),
};

const meta: Meta<StorybookSampleComponent> = {
    title: 'StorybookSample',
    component: StorybookSampleComponent,
    excludeStories: /.*Data$/,
    tags: ['autodocs'],
    render: (args) => ({
        props: {
            ...args,
            onPinTask: actionsData.onPinTask,
            onArchiveTask: actionsData.onArchiveTask,
        },
        template: `<app-storybook-sample ${argsToTemplate(args)}></app-storybook-sample>`,
    }),
};

export default meta;
type Story = StoryObj<StorybookSampleComponent>;

export const Default: Story = {
    args: {
        task: {
            id: '1',
            title: 'Test Task',
            state: 'TASK_INBOX',
        },
    },
};

export const Pinned: Story = {
    args: {
        task: {
            ...Default.args?.task,
            state: 'TASK_PINNED',
        },
    },
};

export const Archived: Story = {
    args: {
        task: {
            ...Default.args?.task,
            state: 'TASK_ARCHIVED',
        },
    },
};