export interface User {
    id?: number;
    username: string;
    email: string;
    password: string;
    phoneNumber: string;
    role?: string;
    token?: string;
}
