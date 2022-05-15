import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalManager } from 'ngb-modal';
import { ToastrService } from 'ngx-toastr';
import { MoUser } from 'src/app/interfaces/interfaces';
import { ApiService } from 'src/app/service/api.service';


@Component({
  selector: 'app-newaccount',
  templateUrl: './newaccount.component.html',
  styleUrls: ['./newaccount.component.css']
})
export class NewaccountComponent implements OnInit {
  @ViewChild('Account') Account: any;
  private _modalRef: any;
  public formAccount: FormGroup;
  public msjError:string = '';
  public msjPass:string = '';
  private oUser:any;

  constructor(
    private _modalService: ModalManager,
    private formBuilder: FormBuilder,
    private _apiService:ApiService,
    private _Toastr: ToastrService
    ) {
    this.formAccount = this.formBuilder.group({
      name: ['', [Validators.required, Validators.pattern(/^[A-Z a-z \s Á-ÿ]+$/)]],
      email: ['', [Validators.pattern(/^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/)]],
      passwd: ['', [Validators.required, Validators.minLength(6)]],
      passwdConf: ['', [Validators.required, Validators.minLength(6)]]
    });    
  }

  ngOnInit(): void { }

  /**
   * OpenAccount Abre modal para crear cuenta desde el login
   */
  public OpenAccount(): void {
    this._modalRef = this._modalService.open(this.Account, {
      size: "md",
      hideCloseButton: true,
      centered: true,
      backdrop: true,
      animation: true,
      keyboard: false,
      closeOnOutsideClick: false,
      backdropClass: "modal-backdrop"
    });
  }

  /**
   * CloseModal cierra el modal AOpenAccount
   */
  public CloseModal(): void {
    this._modalService.close(this.Account)  
  }

  public SendForm = () => {
    this.msjPass='';
    if (this.ConfirmPassword()){
      this.ValidateForm();
      this.CreateAccount();      
    }else {
      this.msjPass = 'Contrasenas no son iguales';
    }

  }

  /**
   * ConfirmPassword: valida que sea las contraseñas iguales
   */
  public ConfirmPassword(): Boolean {
    let bIgual = this.Control['passwd'].value  === this.Control['passwdConf'].value
    // console.log(bIgual);
    return bIgual;
  }


  /**
   * ValidateForm: Valida si el formulario es valido y envia la peticion
   */
  public ValidateForm() {
    this.msjPass='';
    if (this.formAccount.invalid) {
      console.log('Debes ingresar todos los datos necesarios');
      return    
    }
    if (!this.ConfirmPassword()){
      this.msjPass='Las Contraseñas no coinciden';
      return;
    }
    else {
      console.log('Se envio el fomulario');
    }

  }

  public get Control() { return this.formAccount.controls; }

  /**
   * CreateAccount: Metodo que hace la peticion al api
   */
  public CreateAccount() {
    this.oUser = this.formAccount.value;
    console.log(this.oUser);    
    this._apiService.postCreateAccount(this.oUser).subscribe(res => {      
      if(res.sResponse !==null){
        console.log(res.sResponse);
        this._Toastr.success(res.sResponse);
        this.formAccount.reset();
      }else{
        this.msjError = "Ocurrio un problema al registrar el usuario";
      }
    }, error => {      
      this.msjError ="Ocurrio un problema con el servidor vuelva a intentarlo mas tarde.";
    })    
  }

}
