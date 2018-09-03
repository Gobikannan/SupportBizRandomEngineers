import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ShiftsService } from './shifts.service';
import { Engineer } from './engineer';
import * as _ from 'lodash';

@Component({
  selector: 'app-shifts',
  templateUrl: './shifts.component.html',
  styleUrls: ['./shifts.component.less']
})

export class ShiftsComponent implements OnInit {
  shifts = [];
  engineers: Engineer[] = [];
  startDate = new Date();
  endDate = new Date();

  constructor(private shiftsService: ShiftsService, public snackBar: MatSnackBar) {}

  ngOnInit() {
    this.startDateChange();
  }

  calculateEngineerShifts() {
    this.engineers = [];
    for (let i = 0; i < this.shifts.length; i++) {
      for (let j = 0; j < this.shifts[i].shifts.length; j++) {
        const shift = this.shifts[i].shifts[j];
        const erExist = _.find<Engineer>(this.engineers, { Name: shift.engineerName});
        if (erExist) {
          erExist.ShiftTotalCount += 1;
          continue;
        }
        const er = new Engineer();
        er.Name = shift.engineerName;
        er.ShiftTotalCount = 1;
        this.engineers.push(er);
      }
    }
    this.engineers = _.sortBy(this.engineers, [ 'Name' ]);
  }

  setData(data) {
    this.shifts = data;
    this.calculateEngineerShifts();
  }

  fetch() {
    this.shiftsService.getAllShifts(this.startDate, this.endDate).then(response => {
      this.setData(response.data);
    }).catch(err => {
      this.setData([]);
      this.snackBar.open(err.response.data.Message, null, {
        duration: 3000,
      });
    });
  }

  clear() {
    this.shiftsService.clearShift(this.startDate, this.endDate).then(response => {
      this.setData([]);
    }).catch(err => {
      this.setData([]);
      this.snackBar.open('An error occurred while deleting the existing schedule', null, {
        duration: 3000,
      });
    });
  }

  recreate() {
    this.shiftsService.clearShift(this.startDate, this.endDate).then(response => {
      this.fetch();
    }).catch(err => {
      this.setData([]);
      this.snackBar.open('An error occurred while deleting the existing schedule', null, {
        duration: 3000,
      });
    });
  }

  fetchDefault() {
    this.shiftsService.getAllShiftsDefault().then(response => {
      this.setData(response.data);
    }).catch(err => {
      this.setData([]);
      this.snackBar.open(err.response.data.Message, null, {
        duration: 3000,
      });
    });
  }

  clearDefault() {
    this.shiftsService.clearDefaultShift().then(response => {
      this.setData([]);
    }).catch(err => {
      this.setData([]);
      this.snackBar.open('An error occurred while deleting the existing schedule', null, {
        duration: 3000,
      });
    });
  }

  recreateDefault() {
    this.shiftsService.clearDefaultShift().then(response => {
      this.fetchDefault();
    }).catch(err => {
      this.setData([]);
      this.snackBar.open('An error occurred while deleting the existing schedule', null, {
        duration: 3000,
      });
    });
  }

  clealAllSchedules() {
    this.shiftsService.clearAllSchedules().then(response => {
      this.setData([]);
    }).catch(err => {
      this.setData([]);
      this.snackBar.open('An error occurred while deleting all schedule', null, {
        duration: 3000,
      });
    });
  }

  startDateChange() {
    this.endDate = new Date(this.startDate);
    let i = 1;
    do {
        this.endDate.setDate(this.endDate.getDate() + 1);
        const day = this.endDate.getDay();
        // ignore sunday
        if (day === 0 || day === 6) {
          continue;
        }
        i++;
    }
    while (i < 10);
  }
}
