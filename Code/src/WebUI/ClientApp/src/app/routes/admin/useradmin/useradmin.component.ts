import { Component, TemplateRef, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  AppRoleDto, AppUserDto, UserAdminClient, RoleAdminClient, CreateRoleCommand, CreateUserCommand,
  TodoItemsClient, TodoItemDto, TodosVm, TodoListsClient, TodoListDto, UpdateUserCommand, UpdateRoleCommand
} from '../../../SiteManager.V4-api';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Overlay } from '@angular/cdk/overlay';



@Component({
  selector: 'app-admin-useradmin',
  templateUrl: './useradmin.component.html',
  styleUrls: ['./useradmin.component.scss']
})
export class UserAdminComponent implements OnInit{

  debug = false;
  matDialog: MatDialog;

  vm: TodosVm;
  roleList: AppRoleDto[];
  userList: AppUserDto[];

  selectedList: TodoListDto;
  selectedItem: TodoItemDto;

  selectedRole: AppRoleDto;
  selectedUser: AppUserDto;

  roleOptionsEditor: any = {};
  userOptionsEditor: any = {};

  //newRoleModalRef: BsModalRef;
  //roleOptionsModalRef: BsModalRef;
  //userOptionsModalRef: BsModalRef;


  newListEditor: any = {};
  listOptionsEditor: any = {};
  itemDetailsEditor: any = {};

  constructor(private rolesClient: RoleAdminClient,
    private usersClient: UserAdminClient,
    private overlay: Overlay,
    private dialog: MatDialog) {
      this.matDialog = dialog;
  }

  async ngOnInit() {
    this.rolesClient.get().subscribe(
      result => {
        this.roleList = result;
        if (this.roleList.length) {
          this.selectedRole = this.roleList[0];
          this.selectRole(this.selectedRole);
        }
      },
      error => console.error(error)
    );
  }

  showEditUserModal(item: AppUserDto = null): void {

    this.selectedUser = item;
    
    var data = {
      errors: {},
      appUserId: this.selectedUser.appUserId,
      userName: this.selectedUser.userName,
      phoneNumber: this.selectedUser.phoneNumber,
      resetPassword: false,
      password: "",
      confirmPassword: "",
      email: this.selectedUser.email,
      appRoles: this.selectedUser.appRoles,
      roleList: this.roleList
    };

    const dialogConfig = new MatDialogConfig();
    //dialogConfig.maxWidth = '500px';
    dialogConfig.maxHeight = '500px';
    dialogConfig.data = data;
    //dialogConfig.scrollStrategy = this.overlay.scrollStrategies.block();

    var diag = this.dialog.open(DialogEditUserComponent, dialogConfig);
    diag.afterClosed().subscribe(data => {
      //console.log("data", data);
      this.selectRole(this.selectedRole);
    });
  }

  showNewUserModal(): void {

    this.selectedUser = AppUserDto.fromJS({
        appUserId: 0
    });

    var data = {
      errors: {},
      appUserId: this.selectedUser.appUserId,
      userName: this.selectedUser.userName,
      phoneNumber: this.selectedUser.phoneNumber,
      resetPassword: false,
      password: "",
      confirmPassword: "",
      email: this.selectedUser.email,
      appRoles: this.selectedUser.appRoles,
      roleList: this.roleList
    };

    const dialogConfig = new MatDialogConfig();
    //dialogConfig.maxWidth = '500px';
    dialogConfig.maxHeight = '500px';
    dialogConfig.data = data;
    //dialogConfig.scrollStrategy = this.overlay.scrollStrategies.block();

    var diag = this.dialog.open(DialogNewUserComponent, dialogConfig);
    diag.afterClosed().subscribe(data => {
      //console.log("data", data);
      this.selectRole(this.selectedRole);
    });
  }


  showNewRoleModal(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '350px';
    
    var diag = this.dialog.open(DialogNewRoleComponent, dialogConfig);
    diag.afterClosed().subscribe(data => {
      let role = AppRoleDto.fromJS({
        name: data.name,
        appRoleId : data.appRoleId
      });
      this.roleList.push(role);
      this.selectedRole = role;
      // this.newRoleModalRef.hide();
      this.selectRole(role);      
    });
  }

  showEditRoleModal(role: AppRoleDto): void {
    this.selectRole(role); 
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '350px';
    dialogConfig.data = role;

    var diag = this.dialog.open(DialogEditRoleComponent, dialogConfig);
    diag.afterClosed().subscribe(data => {
      let role = AppRoleDto.fromJS({
        name: data.name,
        appRoleId: data.appRoleId
      });
    });
  }

  selectRole(role: AppRoleDto): void {
    this.selectedRole = role;
    //console.log(this.selectedRole); 
    this.usersClient.getByRole(this.selectedRole.appRoleId).subscribe(
      result => {
        this.userList = result;
        if (this.userList.length) {
          this.selectedUser = this.roleList[0];
        }
      },
      error => console.error(error)
    );
  }
}

@Component({
  selector: 'dialog-newrole',
  styles: [
    `
      .demo-full-width {
        width: 100%;
      }
    `,
  ],
  templateUrl: 'dialog_newrole.html',
})
export class DialogNewRoleComponent {
  newRoleName: string;
  error: string
  roleName = new FormControl('', [Validators.required]);
  

  constructor(
    public rolesClient: RoleAdminClient,
    public dialogRef: MatDialogRef<DialogNewRoleComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
   
  }

  addRole(): void {
    let role = AppRoleDto.fromJS({
      name: this.newRoleName
    });

    this.rolesClient.create(<CreateRoleCommand>{ roleName: this.newRoleName }).subscribe(
      result => {
        this.dialogRef.close({ name: this.newRoleName, appRoleId: result });
      },
      error => {
        //let errors = JSON.parse(error.response);

        //if (errors && errors.title) {
        //  this.error = errors.errors.RoleName[0];
        //  this.roleName.setErrors({
        //    server: { message: this.error },
        //  });

        //}
      }
    );
  }
  
}

@Component({
  selector: 'dialog-editrole',
  styles: [
    `
      .demo-full-width {
        width: 100%;
      }
    `,
  ],
  templateUrl: 'dialog_editrole.html',
})
export class DialogEditRoleComponent {
  //newRoleName: string;
  error: string
  roleName = new FormControl('', [Validators.required]);


  constructor(
    public rolesClient: RoleAdminClient,
    public dialogRef: MatDialogRef<DialogNewRoleComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {


    //this.newRoleName = data.name;
  }

  editRole(): void {
    let role = UpdateRoleCommand.fromJS({
      roleName: this.data.name,
      roleId: this.data.appRoleId
    });

    this.rolesClient.update(UpdateRoleCommand.fromJS(role)).subscribe(
      result => {
        this.dialogRef.close(result);
      },
      error => {
        let errors = JSON.parse(error.response);

        if (errors && errors.title) {
          this.error = errors.errors.RoleName[0];
          this.roleName.setErrors({
            server: { message: this.error },
          });
        }
      }

    );
  }

  getErrorMessage() {
    if (this.roleName.hasError('required')) {
      return 'You must enter a name.';
    }

    return this.roleName.hasError('server') ? this.error : '';
  }
}


@Component({
  selector: 'dialog-newuser',
  styles: [``,],
  templateUrl: 'dialog_newuser.html',
})
export class DialogNewUserComponent {
  error: string
  userForm: FormGroup;
  roleList: any[];
  constructor(
    public usersClient: UserAdminClient,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<DialogNewRoleComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    this.userForm = this.fb.group({
      username: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [this.passwordValidator]],
      confirmPassword: ['', [this.confirmValidator]],
      phone: ['', []],
      roles: ['', [Validators.required]],
      sendConfirmationEmail: ['', []],
    });

    this.roleList = data.roleList;
    this.roles.setValue(data.appRoles); //why doesnt patchvalue work ? todo
    this.username.patchValue(data.userName);
    this.email.patchValue(data.email);
    this.phone.patchValue(data.phoneNumber);
    this.sendConfirmationEmail.patchValue(false); 

  }

  get username() {
    return this.userForm.get('username');
  }

  get email() {
    return this.userForm.get('email');
  }

  get password() {
    return this.userForm.get('password');
  }

  get confirmPassword() {
    return this.userForm.get('confirmPassword');
  }

  get phone() {
    return this.userForm.get('phone');
  }

  get sendConfirmationEmail() {
    return this.userForm.get('sendConfirmationEmail');
  }

  get roles() {
    return this.userForm.get('roles');
  }

  createUser(): void {
      var newUserCommand = CreateUserCommand.fromJS({
        userName: this.username.value,
        password: this.password.value,
        confirmPassword: this.confirmPassword.value,
        email: this.email.value,
        phoneNumber: this.phone.value.toString(),
        appRoles: this.roles.value,
        sendRegistrationConfirmation: this.sendConfirmationEmail.value ? true : false
      });
      //console.log("createuser", JSON.stringify(newUserCommand)); 
      this.usersClient.create(newUserCommand)
        .subscribe(
          result => {
            this.data.appUserId = result;
            this.dialogRef.close(result);
          },
          error => {
            console.log(error);
            var e = JSON.parse(error.response);
            for (var err in e.errors) {
              if (e.errors[err]) {
                this.error = e.errors[err];
                break;
              }
            }
          }
        );
  }

  confirmValidator = (control: FormControl): { [k: string]: boolean } => {
    if (!control.value) {
      return { error: true, required: true };
    } else if (control.value !== this.userForm.controls.password.value) {
      return { error: true, confirm: true };
    }
    return {};
  };

  passwordValidator = (control: FormControl): { [k: string]: boolean } => {
    // {6,100}           - Assert password is between 8 and 100 characters
    // (?=.*[0-9])       - Assert a string has at least one number
    if (control.value) {
      var regex = "^(?=.*[0-9])"
        + "(?=.*[a-z])(?=.*[A-Z])"
        + "(?=.*[@#$%^&+=_!])"
        + "(?=\\S+$).{6,20}$";
      if (control.value.match(regex)) {
        //.match(/^(?=.*[0-9])[a-zA-Z0-9!@#$%^&*]{6,100}$/)) {
        return {};
      } else {
        return { error: true, required: true };
      }
    }
  }
}

@Component({
  selector: 'dialog-edituser',
  styles: [
    `
      .demo-full-width {
        width: 100%;
      }
    `,
  ],
  templateUrl: 'dialog_edituser.html',
})
export class DialogEditUserComponent {
  error: string
  userForm: FormGroup;
  roleList: any[]; 
  constructor(
    public usersClient: UserAdminClient,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<DialogNewRoleComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    this.userForm = this.fb.group({
      username: ['', [Validators.required]],
      email: ['', [Validators.required,Validators.email]],
      phone: ['', []],
      roles: ['', [Validators.required]],
      resetPassword: ['', []],
    });
    
    this.roleList = data.roleList;
    this.roles.setValue(data.appRoles); //why does patchvalue not work ? todo
    this.username.patchValue(data.userName);
    this.email.patchValue(data.email);
    this.phone.patchValue(data.phoneNumber);

    
  }

  get username() {
    return this.userForm.get('username');
  }

  get email() {
    return this.userForm.get('email');
  }

  get password() {
    return this.userForm.get('password');
  }

  get confirmPassword() {
    return this.userForm.get('confirmPassword');
  }

  get phone() {
    return this.userForm.get('phone');
  }

  get resetPassword() {
    return this.userForm.get('resetPassword');
  }

  get roles() {
    return this.userForm.get('roles');
  }

  updateUser(): void {
      var updateUserCommand = UpdateUserCommand.fromJS({
        appUserId: this.data.appUserId,
        userName: this.username.value,
        email: this.email.value,
        phoneNumber: this.phone.value.toString(),
        appRoles: this.roles.value,
        resetPassword: this.resetPassword.value ? true : false
      });

      this.usersClient.update(updateUserCommand)
        .subscribe(
          result => {
            this.dialogRef.close(result);
          },
          error => {
            console.log(error);
            var e = JSON.parse(error.response);
            for (var err in e.errors) {
              if (e.errors[err]) {
                this.error = e.errors[err];
                break;
              }
            }
          }
        );
  }
}
