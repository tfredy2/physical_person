import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AllRegister, ResponseToka } from '../interfaces/interfaces';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  body = {
    Username: 'ucand0021',
    Password:'yNDVARG80sr@dDPc2yCT!'
  }
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      // Authorization: 'Bearer ' + this._Cookie.get('sToken')
    })
  };


  constructor(private _http:HttpClient) { }
  private _URL:string='https://api.toka.com.mx/candidato/api/login/authenticate'
  private _costumer:string ='https://api.toka.com.mx/candidato/api/customers'

  sToken = ():Observable<ResponseToka> =>{
    return this._http.post<ResponseToka>(this._URL,JSON.stringify(this.body) ,this.httpOptions);
  }

  AllRegister= (sToken:string):Observable<AllRegister> =>{
    let header = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: 'Bearer '+sToken
      })
    };
    return this._http.get<AllRegister>(this._costumer, header);
  }

}
