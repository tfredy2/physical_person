import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule, HttpEvent, HttpHeaders, HttpParams } from '@angular/common/http';
import { MoUser, PersonInfo, CreateAccount, MoPhysicalPerson, oLsPerson, PhysicalPerson, PhysicalPerson_dos, AddRespose } from '../interfaces/interfaces'
import { observable, Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  // private httpOptionsAouth = {
  //   headers: new HttpHeaders({
  //     'Content-Type':  'application/json',
  //     Authorization: 'Bearer '
  //   })
  // };

  constructor(private _http: HttpClient, private _Cookie: CookieService) { }

  private _URL: string = "https://localhost:44332/api/";
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + this._Cookie.get('sToken')
    })
  };

  public getPhysicalPerson = (oUSer: MoUser): Observable<PersonInfo> => {
    return this._http.post<PersonInfo>(this._URL + "Login/DoLogin", JSON.stringify(oUSer), this.httpOptions);
  }
  public postCreateAccount = (oUSer: MoUser): Observable<CreateAccount> => {
    return this._http.post<CreateAccount>(this._URL + 'Login/CreateAccount', JSON.stringify(oUSer), this.httpOptions);
  }

  public AllPerson = (): Observable<oLsPerson> => {
    console.log('esto es el valor de mi cookie', this._Cookie.get('sToken'));
    return this._http.get<oLsPerson>(this._URL + 'Person/ListPerson', this.httpOptions);
  }

  CreatePersonPhysical = (oPerson:PhysicalPerson):Observable<AddRespose> =>{
    // debugger;
    return this._http.post<AddRespose>(this._URL+'Person/AddPerson',oPerson,this.httpOptions);
  }
  
  public DeletePerson = (person:PhysicalPerson):Observable<AddRespose> =>{
    console.log(this._Cookie.get('sToken'));
    // let id= person.physicalPerson.ID;
    console.log(this._http.delete(this._URL+'Person/DeletePerson'+person,this.httpOptions).toString());
    return this._http.delete<AddRespose>(this._URL+'Person/DeletePerson'+person,this.httpOptions);
  }

  DeleteEmployee(physicalPersons: PhysicalPerson_dos):Observable<AddRespose> {  
    let id = physicalPersons.ID
    return this._http.delete<AddRespose>(this._URL+'Person/DeletePerson/'+id,this.httpOptions)  
   }

   UpdatePerson = (person:any):Observable<AddRespose>=>{
     return this._http.put<AddRespose>(this._URL+'Person/UpdatePerson',person,this.httpOptions)
   }


}
