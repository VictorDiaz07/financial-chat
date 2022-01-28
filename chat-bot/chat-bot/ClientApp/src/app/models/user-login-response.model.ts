export interface UserLoginResponse {
    isAuthSuccessful: boolean;
    errorMessage: string;
    token: string;
}