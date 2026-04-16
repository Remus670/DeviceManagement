export interface Device {
  id: number;
  name: string;
  manufacturer: string;
  type: string;
  operatingSystem: string;
  osVersion: string;
  processor: string;
  ramAmount: string;
  description: string;
  userId?: number;
  user?: User;
}

export interface User {
  id: number;
  name: string;
  role: string;
  location: string;
}