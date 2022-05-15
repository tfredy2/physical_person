import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalManager } from 'ngb-modal';
import { oLsPerson,PhysicalPerson } from 'src/app/interfaces/interfaces';
import { ApiService } from 'src/app/service/api.service';
import * as FileSaver from 'file-saver';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { ToastrService } from 'ngx-toastr';



@Component({
  selector: 'app-person',
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {
  persons: any = [];
  public sTitulo: string = '';
  private sFecha:string ='/^\d{2}\/\d{2}\/\d{4}$/'
  public formPerson: FormGroup;
  private _registro:any=[]
  public nombre:string = ''; 
  pagina: number = 1;
  @ViewChild('Persona') Account: any;
  @ViewChild('Delete') Delete: any;
  private _modalRef: any;
  private _modalRefDel: any;
  constructor(private _http: ApiService,
    private _modalService: ModalManager,
    private _formBuilder: FormBuilder,
    private _Cookie: CookieService,
    private _Toastr: ToastrService
  ) {
    this.formPerson = this._formBuilder.group({
      ID:[0],
      Activo:[true],
      Nombre: ['', [Validators.required, Validators.minLength(4)]],
      ApellidoPaterno: ['',Validators.required],
      ApellidoMaterno: ['',Validators.required],
      RFC: ['',[Validators.required,Validators.minLength(13),Validators.maxLength(13)]],
      FechaNacimiento: ['',Validators.required],
      UsuarioAgrega:[Number(this._Cookie.get('sId'))],
      FechaRegistro:[null],
      FechaActualizacion:[null]
    });
  }

  ngOnInit(): void {
    this.allPerson();
  }

  allPerson = () => {
    this._http.AllPerson().subscribe(e => {
      this.persons = e.oLsPerson;
      console.log(e);
    }, errr => {
      console.log(errr);
    })
  }

  Person = (opcion: number,reg:any=null) => {
    this.OpenModal();
    this.sTitulo = opcion === 0 ? 'Agregar ' : 'Editar';
    if (opcion===1) {      
      this.EditPerson(reg);
    }    
  }

  EditPerson=(registro:[])=>{
    this.formPerson.setValue(registro)    
    console.log('Vamos a  editar una persona',this.formPerson.value);
  }

  CloseModal = () => {
    this.formPerson.reset();
    this._modalRef.close();
  }

  DeletePerson = (register:any) => {    
    this._registro = register;
    this.nombre = this._registro.Nombre;
    this.DeleteModal()
    console.log(this._registro);        
    // this.DeleteModal();
  }

  OpenModal = () => {
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

  DeleteModal = () => {
    console.log('sdfsdf');
    this._modalRefDel = this._modalService.open(this.Delete, {
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

  CloseDelete() {
    this._modalRefDel.close();
  }

  DeleteServiceModal= () =>{
    // debugger;
    console.log(this._registro);    
    this.DeleteService(this._registro)
    // this._registro=[]  
  }


  DeleteService = (register:any) => {
    this._http.DeleteEmployee(register).subscribe(e =>{
      // this.persons.pop(register);
      console.log(e.Mensaje);
      this.persons = e.oLsPerson;
      this._Toastr.error(e.Mensaje);
      this.CloseDelete();

    },err =>{
      this._Toastr.error('Error del servidor, favor de intentar mas tarde');
      console.log(err);
    })
    console.log('Se elimino el registro');
    // this.CloseDelete();
  }

  public ExportExcel() {
    if (this.persons.length > 0) {
      import("xlsx").then(xlsx => {
        const worksheet = xlsx.utils.json_to_sheet(this.persons);
        const workbook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
        const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
        this.saveAsExcelFile(excelBuffer, "Personas_Fisicas");
        this._Toastr.success('Registros Exportados a Excel');
      });
    }
  }

  public get Control() { return this.formPerson.controls; }

  SendForm = () => {
    if (this.formPerson.invalid) {
      console.log('Formulario invalido');      
      return;
    }    

    // debugger;
    if (this.sTitulo==='Editar') {
      console.log(this.sTitulo);
      debugger;
      this._http.UpdatePerson(this.formPerson.value).subscribe(data =>{
        this.persons = data.oLsPerson;
        this._Toastr.success(data.Mensaje);
        // console.log(data);        
      },err=>{
        this._Toastr.error('Ocurrio un error al intentar actualizar el registro');
      })
      
    }else{
      this._http.CreatePersonPhysical(this.formPerson.value).subscribe(data=>{
        this.persons = data.oLsPerson
        this._Toastr.warning(data.Mensaje);
      },err=>{
        this._Toastr.success('Ocurrio un error en el servidor, vuelva a intentarlo en unos minutos');
        console.log(err);
      })
    }    
    this._modalRef.close();
    this.formPerson.reset();
  }


  saveAsExcelFile(buffer: any, fileName: string): void {
    let EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    let EXCEL_EXTENSION = '.xlsx';
    const data: Blob = new Blob([buffer], {
      type: EXCEL_TYPE
    });
    FileSaver.saveAs(data, fileName + EXCEL_EXTENSION);
  }
}
