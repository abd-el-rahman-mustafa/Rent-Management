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


export enum Gender
{
    NotSpecified,
    Male,
    Female
}

