export enum CategoryVehicle {
    MicroUrbaine,
    MiniCitadine,
    CitadinePolyvalente,
    Compacte,
    BerlineTailleS,
    BerlineTailleM,
    BerlineTailleL,
    SUVPickUpTT
}

export enum MotorizationVehicle {
    Diesel,
    Essence,
    Ethanol,
    Hybride,
    Electrique
}

export enum StatusVehicle {
    Service,
    HorsService,
    Reparation
}

export default interface VehicleInterface {
    id: number;
    registration: string;
    brand: string;
    model: string;
    category: CategoryVehicle;
    urlPhoto: string;
    motorization: MotorizationVehicle;
    cO2: number;
    status: StatusVehicle;
    nbPlaces: number;
    // rents: list string;
}
