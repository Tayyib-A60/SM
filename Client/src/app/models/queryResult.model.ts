import { Vehicle } from './vehicle.model';

export interface QueryResult {
  totalItems: number;
  vehicles: Vehicle[];
}
