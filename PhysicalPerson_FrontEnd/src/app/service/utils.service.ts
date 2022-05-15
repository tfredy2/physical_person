import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {
  public nombre: string ='';

  constructor(private _Cookie: CookieService) { }


  SetToken = (sToken:string,nombre:string,id:string) =>{
    this._Cookie.set('sToken',sToken);
    this._Cookie.set('sName',nombre);
    this._Cookie.set('sId',id);
  }

  DeleteCookie=()=>{
    this._Cookie.deleteAll();
  }

  NameUser =(nombre:string)=>{
    return this.nombre = this._Cookie.get('sName');
  }
  // NameUser= ()=> this.User()

}
