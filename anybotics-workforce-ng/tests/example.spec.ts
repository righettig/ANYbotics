import { test, expect } from '@playwright/test';

test('basic test', async ({ page }) => {
    await page.goto('http://localhost:4200');
    await expect(page).toHaveTitle(/AnyboticsWorkforceNg/);
});
