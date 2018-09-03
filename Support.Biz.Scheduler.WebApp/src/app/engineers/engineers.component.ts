import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, MatTableDataSource, MatSnackBar } from '@angular/material';
import { EngineersService } from './engineers.service';

@Component({
  selector: 'app-engineers',
  templateUrl: './engineers.component.html',
  styleUrls: ['./engineers.component.less']
})
export class EngineersComponent implements OnInit {
  displayedColumns: string[] = ['firstName', 'lastName', 'isActive'];
  dataSource = new MatTableDataSource([]);

  @ViewChild(MatSort) sort: MatSort;

  constructor(private engineersService: EngineersService, public snackBar: MatSnackBar) {}

  ngOnInit() {
    this.engineersService.getAllEngineers().then(response => {
      this.dataSource = new MatTableDataSource(response.data);
      this.dataSource.sort = this.sort;
    }).catch(err => {
      console.log('An error occurred whilte fetching Engineers..');
      this.snackBar.open('An error occurred whilte fetching Engineers..', null, {
        duration: 3000,
      });
    });
  }
}
