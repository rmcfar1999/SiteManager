import { Component, OnInit } from '@angular/core';
import { TodoListsClient, TodoItemsClient, TodoItemDto, TodosVm, WeatherForecastClient, WeatherForecast } from '../../SiteManager.V4-api'

@Component({
  selector: 'app-error-403',
  template: `
    <error-code
      code="403"
      [title]="'Permission denied!'"
      [message]="'You do not have permission to access the requested data.'"
    ></error-code>
  `,
})


export class Error403Component implements OnInit {


  constructor(private listClient: TodoListsClient, private weatherClient: WeatherForecastClient) {
      

  }

  async ngOnInit() {
    //var lists = this.weatherClient.get().subscribe(function (resp) {
    //  console.log(resp); 

    //});

    //var todos = this.listClient.get().subscribe(function (resp) {
    //  console.log(resp);
    //});

  }

}
