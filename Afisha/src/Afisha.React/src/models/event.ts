export interface OutputEventBase {
  id: number;
  name: string;
  organizer: string;
  location: string;
  date: string; // DateOnly преобразуется в string в JSON
  participants: number[]; // Массив Id участников
}

export interface OutputEvent extends OutputEventBase {
  // Дополнительные поля отсутствуют, но оставляем для совместимости
}

export interface PagedEventsResponse {
  events: OutputEventBase[];
  totalCount: number;
  totalPages: number;
}

export interface OutputLocation {
  id: number;
  name: string;
  xCoordinate: number;
  yCoordinate: number;
}