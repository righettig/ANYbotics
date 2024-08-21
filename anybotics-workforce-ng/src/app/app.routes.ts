import { Routes } from '@angular/router';
import { AgentsComponent } from './agents/agents.component';
import { AgentDetailsComponent } from './agent-details/agent-details.component';
import { LoginComponent } from './login/login.component';
import { CommandsComponent } from './commands/commands.component';
import { NotAuthorizedComponent } from './not-authorized/not-authorized.component';
import { MissionsComponent } from './missions/missions.component';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/agents', pathMatch: 'full' },
  { path: 'not-authorized', component: NotAuthorizedComponent },
  { path: 'login', component: LoginComponent },
  { path: 'agents', component: AgentsComponent, canActivate: [AuthGuard] },
  { path: 'agents/:id', component: AgentDetailsComponent, canActivate: [AuthGuard] },
  { path: 'commands/:id', component: CommandsComponent, canActivate: [AuthGuard] },
  { path: 'missions/:id', component: MissionsComponent, canActivate: [AuthGuard] },
  {
    path: 'admin',
    loadComponent: () => import('./admin/admin.component').then(m => m.AdminComponent),
    canActivate: [AdminGuard]
  },
];
