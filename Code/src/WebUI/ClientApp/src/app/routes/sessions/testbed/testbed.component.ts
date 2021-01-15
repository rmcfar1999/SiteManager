import { Component, OnInit } from '@angular/core';
import { AuthorizeService, AuthenticationResultStatus } from '../../../core/authentication/authorize.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, concat, from, Observable, Subscription } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  AppRoleDto, AppUserDto, UserAdminClient, RoleAdminClient, CreateRoleCommand, CreateUserCommand,
  TodoItemsClient, TodoItemDto, TodosVm, TodoListsClient, TodoListDto, UpdateUserCommand, UpdateRoleCommand, WeatherForecast, WeatherForecastClient
} from '../../../SiteManager.V4-api';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Overlay } from '@angular/cdk/overlay';
import { MenuService } from '../../../core/bootstrap/menu.service';

@Component({
  selector: 'app-sessions-testbed',
  templateUrl: './testbed.component.html',
  styleUrls: ['./testbed.component.scss']
})
export class SessionsTestbedComponent implements OnInit {

  public authService: AuthorizeService;
  public message: string[];

  constructor(authService: AuthorizeService, private apiClient: WeatherForecastClient, private menu: MenuService) {
    this.authService = authService;
    this.message = [];
  }

  

  ngOnInit() {
    //this.menu.getAll().subscribe(menu => {
    //  console.log("menu", menu);
    //});
    this.authService.getUser().pipe(take(1))
      .subscribe(user => {
        console.log("user profile", user.profile);
        console.log("user", user);
        
        this.menu.getAll().subscribe(menu => {
          console.log("menu", menu);
          this.menu.recursMenuForPermissions(menu, user.role);
        });

        this.message.push(user.name + "(" + user.sub + ") : " + user.role);
      });



    //this.apiClient.get()
    //  .subscribe(res => {
    //    console.log("result", res);
    //    this.message = res.map(item => item.summary);
    //  });


    //this.message = "bob"; 
  }

}
