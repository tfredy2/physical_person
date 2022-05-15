import { Component, OnInit } from '@angular/core';
import * as FileSaver from 'file-saver';
import { ReportService } from 'src/app/service/report.service';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
  public costumers: any = []
  pagina: number = 1;
  constructor(private _report: ReportService) { }

  ngOnInit(): void {
    this.tokenToka();
  }

  tokenToka = () => {
    this._report.sToken().subscribe(e => {
      if (e.Data !== null) {
        this.AllResgister(e.Data)
      }
    }, err => {
      console.log(err);
    });
  }

  AllResgister = (sToken: string) => {
    this._report.AllRegister(sToken).subscribe(e => {
      if (e.Data != null) {
        this.costumers = e.Data;
      }
    }, err => {
      alert('Ocurrio un error');
      console.log(err);
    })
  }

  public ExportExcel() {
    if (this.costumers.length > 0) {
      import("xlsx").then(xlsx => {
        const worksheet = xlsx.utils.json_to_sheet(this.costumers);
        const workbook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
        const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
        this.saveAsExcelFile(excelBuffer, "Costumers");
      });
    }
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
