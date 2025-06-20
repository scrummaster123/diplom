export interface OutputEventBase {
  id: number;
  name: string;
  organizer: string;
  location: string;
  date: string; // DateOnly преобразуется в string в JSON
}

export interface OutputEvent extends OutputEventBase {
  // Дополнительные поля отсутствуют, но оставляем для совместимости
}

export interface PagedEventsResponse {
  events: OutputEventBase[];
  totalCount: number;
  totalPages: number;
}