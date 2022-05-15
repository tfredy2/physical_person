export interface MoUser {
    oUser: {
        id: number;
        name: string;
        email: string;
        passwd: string;
        fechaRegistro: Date
    };
}

export interface PersonInfo {
    PersonInfo: {
        ID: number;
        Email: string;
        Name: string;
        UserToken: string;
        Duration: Date;
    };
}

export interface CreateAccount {
    sResponse:string;
}


export interface MoPhysicalPerson{
    oLsPerson: {
        ID:number;
        Name:string;
        Nombre:string;
        ApellidoPaterno:string;
        ApellidoMaterno:string;
        RFC:string;
        FechaNacimiento:Date;
        UsuarioAgrega:number;
        Activo:boolean;
        FechaRegistro:Date;
        FechaActualizacion:Date;
    }    
}


export interface oLsPerson{
    oLsPerson:PhysicalPerson[];
}

export interface PhysicalPerson{
    physicalPerson: {
        ID:number;
        Name:string;
        Nombre:string;
        ApellidoPaterno:string;
        ApellidoMaterno:string;
        RFC:string;
        FechaNacimiento:Date;
        UsuarioAgrega:number;
        Activo:boolean;
        FechaRegistro:Date;
        FechaActualizacion:Date;
    }   
}


export interface PhysicalPerson_dos{
        ID:number;
        Name:string;
        Nombre:string;
        ApellidoPaterno:string;
        ApellidoMaterno:string;
        RFC:string;
        FechaNacimiento:Date;
        UsuarioAgrega:number;
        Activo:boolean;
        FechaRegistro:Date;
        FechaActualizacion:Date;
}

export interface ResponseToka{
    Data:string;
}


export interface AllRegister{
    Data:[{
        IdCliente:number,
        FechaRegistroEmpresa:Date,
        RazonSocial:string,
        RFC: string,
        Sucursal:string,
        IdEmpleado: number,
        Nombre: string,
        Paterno:string,
        Materno: string,
        IdViaje: number
    }]
}

export interface AddRespose{
    Mensaje:string;
    oLsPerson:PhysicalPerson[]
}