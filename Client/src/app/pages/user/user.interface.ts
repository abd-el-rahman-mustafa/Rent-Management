import { UserRole } from "../role/role.interface";

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    emailConfirmed: boolean;
    phoneNumber?: string;
    phoneNumberConfirmed: boolean;
    isActive: boolean;
    dateOfBirth?: Date;
    createdAt: Date;
    updatedAt: Date;
    isDeleted: boolean;
    userRoles: UserRole[];
    gender: Gender;
}

export interface SimpleUserInfo {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
}
export interface UpdateUserDto {
  firstName:   string;
  lastName:    string;
  email:       string;
  phone?:      string;
  gender?:     Gender;
  dateOfBirth?: string;  // ISO date string "YYYY-MM-DD"
  active?:     boolean;
}
export enum Gender
{
    NotSpecified,
    Male,
    Female
}

