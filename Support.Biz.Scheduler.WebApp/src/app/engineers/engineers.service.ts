import { Injectable } from '@angular/core';
import axios from 'axios';
import { environment } from '../../environments/environment';

@Injectable()
export class EngineersService {
  private baseUrl = environment.baseUrl;
  private engineersUrl = this.baseUrl + 'api/engineers';

  getAllEngineers(): any {
    return axios.get(this.engineersUrl);
  }
}
