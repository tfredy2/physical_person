import { Component, OnInit, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/service/api.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { UtilsService } from 'src/app/service/utils.service';
import { NewaccountComponent } from '../newaccount/newaccount.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @ViewChild(NewaccountComponent) Account:any;
  private _regexEmail = new RegExp(/^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/);
  public formGroup: FormGroup;
  public msjError: string = '';

  constructor(private _login: ApiService,
    private _spinner: NgxSpinnerService,
    private formBuilder: FormBuilder,
    private _router: Router,
    private _utilService: UtilsService
  ) {
    this.formGroup = this.formBuilder.group({
      email: ['', [Validators.required, Validators.pattern(this._regexEmail)]],
      passwd: ['', [Validators.required, Validators.min(5)]]
    });
  }

  ngOnInit(): void {
  }


  login = (): void => {
    this.msjError = "";
    if (this.formGroup.invalid) {
      this.msjError = "Necesitas ingresar los datos correspondientes";
      // console.log('Formulario invalido');
      return;
    } else {
      this._login.getPhysicalPerson(this.formGroup.value).subscribe(data => {
        console.log(data.PersonInfo.UserToken)
        if (data.PersonInfo.Email != null) {
          this._utilService.SetToken(data.PersonInfo.UserToken,data.PersonInfo.Name,data.PersonInfo.ID.toString());
          this._utilService.NameUser(data.PersonInfo.Name)
          this._router.navigate(['/home']);
        } else {
          this.msjError = "Usuario o contraseña no valido";
        }
      }, err => {
        this.msjError = "Servicio no disponible, intente de nuevo en unos minutos.";
      });
    }
  }

  AbrirModal = (): void => {
    console.log('Funciona')
    this.Account.OpenAccount();
  }



  public get Control() { return this.formGroup.controls; }
}
