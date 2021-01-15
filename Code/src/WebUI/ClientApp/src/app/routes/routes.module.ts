import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { RoutesRoutingModule } from './routes-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';


const COMPONENTS = [DashboardComponent];//, LoginUIComponent, RegisterComponent];
const COMPONENTS_DYNAMIC = [];

@NgModule({
  imports: [SharedModule, RoutesRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
  entryComponents: COMPONENTS_DYNAMIC,
})
export class RoutesModule {}
