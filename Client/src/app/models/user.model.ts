export interface User {
  id: number;
  name: string;
  email: string;
  password: string;
  role: Role;
  }
  export enum Role {
    Admin,
    Customer
  }
