import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';
import { UserAdminComponent, DialogNewRoleComponent, DialogEditRoleComponent, DialogNewUserComponent, DialogEditUserComponent } from './useradmin/useradmin.component';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';


const COMPONENTS = [UserAdminComponent];
const COMPONENTS_DYNAMIC = [DialogNewRoleComponent, DialogEditRoleComponent, DialogNewUserComponent,DialogEditUserComponent];

@NgModule({
  imports: [
    SharedModule,
    CommonModule,
    AdminRoutingModule
  ],
  declarations: [
    ...COMPONENTS,
    ...COMPONENTS_DYNAMIC
  ],
  entryComponents: COMPONENTS_DYNAMIC
})
export class AdminModule { }
