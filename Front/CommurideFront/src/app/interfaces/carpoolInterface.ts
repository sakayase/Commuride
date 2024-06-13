import { VehicleComponent } from "../components/vehicle/vehicle.component";
import VehicleInterface from "./vehicleInterface";

export interface carpoolInterface {
    id: number;
    addressLeaving: string;
    dateDepart: Date;
    addressArrival: string;
    duration: any;
    distance: number;
    driver: string;
    // passengers
    vehicleDTO: VehicleInterface
}
