export interface LoginUserModel {
  email: string;
  password: string;
}

export interface RegistrationUserModel {
  firstName: string;
  lastName: string;
  patronymic?: string;
  email: string;
  login: string;
  password: string;
  confirmPassword: string;
  birthday?: string; // Используем ISO-строку для даты
  isMale?: boolean;
}