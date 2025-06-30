export interface OutputLocationBase {
  ownerId: number;
  name: string;
  pricing: number;
  isWarmPlace: boolean;
}

export interface OutputLocationFull extends OutputLocationBase {
  events: string[];
}

export interface PagedLocationsResponse {
  locations: OutputLocationBase[];
  totalCount: number;
  totalPages: number;
}