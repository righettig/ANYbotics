import { Routes } from '@angular/router';
import { AgentsComponent } from './agents/agents.component';
import { AgentDetailsComponent } from './agent-details/agent-details.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
import { CommandsComponent } from './commands/commands.component';

export const routes: Routes = [
  { path: '', redirectTo: '/agents', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'agents', component: AgentsComponent, canActivate: [AuthGuard] },
  { path: 'agents/:id', component: AgentDetailsComponent, canActivate: [AuthGuard] },
  { path: 'commands/:id', component: CommandsComponent, canActivate: [AuthGuard] },
  {
    path: 'admin',
    loadComponent: () => import('./admin/admin.component').then(m => m.AdminComponent),
    canActivate: [AuthGuard]
  },
];
