export interface AnimalType {
  id: number;
  name: string;
}

export interface CreateAnimalTypeRequest {
  name: string;
}

export interface Breed {
  id: number;
  name: string;
  animalTypeId: number;
  animalTypeName: string;
}

export interface CreateBreedRequest {
  name: string;
  animalTypeId: number;
}

export interface Animal {
  id: number;
  inventoryNumber: string;
  gender: string;
  name: string;
  arrivalDate: string;
  arrivalAgeMonths: number;
  breedId: number;
  breedName: string;
  parentAnimalId: number | null;
  parentAnimalName: string | null;
}

export interface CreateAnimalRequest {
  inventoryNumber: string;
  gender: string;
  name: string;
  arrivalDate: string;
  arrivalAgeMonths: number;
  breedId: number;
  parentAnimalId: number | null;
}

export interface Weighting {
  id: number;
  animalId: number;
  animalName: string;
  userId: string;
  userName: string;
  date: string;
  weightKg: number;
}

export interface CreateWeightingRequest {
  animalId: number;
  date: string;
  weightKg: number;
}

export interface UserDto {
  id: string;
  userName: string;
  email: string;
  isActive: boolean;
  roles: string[];
}
