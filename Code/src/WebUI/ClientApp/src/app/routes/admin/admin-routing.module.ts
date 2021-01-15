import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserAdminComponent } from './useradmin/useradmin.component';

const routes: Routes = [{ path: 'useradmin', component: UserAdminComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
