import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';

export interface MenuTag {
  color: string; // Background Color
  value: string;
}

export interface MenuChildrenItem {
  route: string;
  visible: boolean;
  name: string;
  roles?: string[];
  type: 'link' | 'sub' | 'extLink' | 'extTabLink';
  children?: MenuChildrenItem[];
}

export interface Menu {
  route: string;
  visible: boolean;
  name: string;
  type: 'link' | 'sub' | 'extLink' | 'extTabLink';
  icon: string;
  roles?: string[];
  label?: MenuTag;
  badge?: MenuTag;
  children?: MenuChildrenItem[];
}

@Injectable({
  providedIn: 'root',
})
export class MenuService {
  private isEven = (menu) => menu.visible;
  private menu$: BehaviorSubject<Menu[]> = new BehaviorSubject<Menu[]>([]);
  private filter$: BehaviorSubject<Menu[]> = new BehaviorSubject<Menu[]>([]);

  getAll(): Observable<Menu[]> {
    return this.menu$.asObservable();
  }

  getVisible(): Observable<Menu[]> {
    return this.menu$.asObservable().pipe(map(menu => menu.filter(m => m.visible)));
  }

  isVisible(menu: Menu[] | MenuChildrenItem[]): boolean {
    return menu.length > 1; //visible;
  }

  set(menu: Menu[]): Observable<Menu[]> {
    this.menu$.next(menu);
    return this.menu$.asObservable();
  }

  add(menu: Menu) {
    const tmpMenu = this.menu$.value;
    tmpMenu.push(menu);
    this.menu$.next(tmpMenu);
  }

  reset() {
    this.menu$.next([]);
  }

  getMenuItemName(routeArr: string[]): string {
    return this.getMenuLevel(routeArr)[routeArr.length - 1];
  }

  /** Menu level */

  private isLeafItem(item: MenuChildrenItem): boolean {
    //// if a menuItem is leaf
    const cond0 = item.route === undefined;
    const cond1 = item.children === undefined;
    const cond2 = !cond1 && item.children.length === 0;
    return cond0 || cond1 || cond2;
  }

  private deepcopyJsonObj(jobj: any): any {
    //// deepcop object-could-be-jsonized
    return JSON.parse(JSON.stringify(jobj));
  }

  private jsonObjEqual(jobj0: any, jobj1: any): boolean {
    //// if two objects-could-be-jsonized equal
    const cond = JSON.stringify(jobj0) === JSON.stringify(jobj1);
    return cond;
  }

  private routeEqual(routeArr: Array<string>, realRouteArr: Array<string>): boolean {
    //// if routeArr equals realRouteArr(after remove empty-route-element)
    realRouteArr = this.deepcopyJsonObj(realRouteArr);
    realRouteArr = realRouteArr.filter(r => r !== '');
    return this.jsonObjEqual(routeArr, realRouteArr);
  }

  getMenuLevel(routeArr: string[]): string[] {
    let tmpArr = [];
    this.menu$.value.forEach(item => {
      //// breadth-first-traverse -modified
      let unhandledLayer = [{ item, parentNamePathList: [], realRouteArr: [] }];
      while (unhandledLayer.length > 0) {
        let nextUnhandledLayer = [];
        for (const ele of unhandledLayer) {
          const eachItem = ele.item;
          const currentNamePathList = this.deepcopyJsonObj(ele.parentNamePathList).concat(
            eachItem.name
          );
          const currentRealRouteArr = this.deepcopyJsonObj(ele.realRouteArr).concat(eachItem.route);
          //// compare the full Array
          //// for expandable
          const cond = this.routeEqual(routeArr, currentRealRouteArr);
          if (cond) {
            tmpArr = currentNamePathList;
            break;
          }

          const isLeafCond = this.isLeafItem(eachItem);
          if (!isLeafCond) {
            const children = eachItem.children;
            const wrappedChildren = children.map(child => ({
              item: child,
              parentNamePathList: currentNamePathList,
              realRouteArr: currentRealRouteArr,
            }));
            nextUnhandledLayer = nextUnhandledLayer.concat(wrappedChildren);
          }
        }
        unhandledLayer = nextUnhandledLayer;
      }
    });
    return tmpArr;
  }

  /** Menu for translation */

  recursMenuForTranslation(menu: Menu[] | MenuChildrenItem[], namespace: string) {
    menu.forEach(menuItem => {
      menuItem.name = `${namespace}.${menuItem.name}`;
      if (menuItem.children && menuItem.children.length > 0) {
        this.recursMenuForTranslation(menuItem.children, menuItem.name);
      }
    });
  }

  /** sort menu visibility by base roles */
  recursMenuForPermissions(menu: Menu[] | MenuChildrenItem[], userRoles: string[]) {
    menu.forEach(menuItem => {
      var allowedRoles = this.hasPermissions(menuItem.roles, userRoles);
      if (allowedRoles.length > 0) {
        menuItem.visible = true;
      } else {
        menuItem.visible = false; 
      }
      if (menuItem.children && menuItem.children.length > 0) {
        this.recursMenuForPermissions(menuItem.children, userRoles);
      }
    });
  }

  hasPermissions(menuRoles : string[], userRoles : string[]) : string[] {
    var ret = [];
    menuRoles = menuRoles.sort().map(e => e.toLowerCase());
    //note that userroles from the oidc-client(json) can be a string vs a string[]?
    if (Array.isArray(userRoles)) {
      userRoles = userRoles.sort().map(e => e.toLowerCase());
    } else { //todo: strange typescript issue with map/userroles "property does not exist on type 'never'"
      var foo = [];
      foo.push(userRoles);
      userRoles = foo.map(e => e.toLowerCase());
    }
    for (var i = 0; i < menuRoles.length; i += 1) {
      if (userRoles.indexOf(menuRoles[i]) > -1) {
        ret.push(this[i]);
      }
    }
    return ret;
  };
}
