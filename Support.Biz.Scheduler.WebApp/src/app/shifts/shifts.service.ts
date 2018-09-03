import { Injectable } from '@angular/core';
import axios from 'axios';
import { environment } from 'src/environments/environment';

@Injectable()
export class ShiftsService {
  private baseUrl = environment.baseUrl;
  private shiftsUrl = this.baseUrl + 'api/shift';

  getAllShifts(startDate: Date, endDate: Date): any {
    return axios.get(this.shiftsUrl + '/' + startDate.toDateString() + '/' + endDate.toDateString());
  }

  getAllShiftsDefault(): any {
    return axios.get(this.shiftsUrl);
  }

  clearDefaultShift(): any {
    return axios.delete(this.shiftsUrl);
  }

  clearShift(startDate: Date, endDate: Date): any {
    return axios.delete(this.shiftsUrl + '/' + startDate.toDateString() + '/' + endDate.toDateString());
  }

  clearAllSchedules(): any {
    return axios.delete(this.shiftsUrl + '/delete/all/schedules');
  }
}
