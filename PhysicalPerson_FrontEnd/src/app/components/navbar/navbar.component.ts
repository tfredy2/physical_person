import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UtilsService } from 'src/app/service/utils.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  public nombre:string="";

  constructor(private _utils:UtilsService,
              private _router: Router
              ) { }

  ngOnInit(): void {
    this.nombre = this._utils.nombre; 
  }


  cerrarSesion = () =>{
    this._utils.DeleteCookie();
    this._router.navigate(['/']);
    // console.log('Cerrar Sesion');    
  }

}
