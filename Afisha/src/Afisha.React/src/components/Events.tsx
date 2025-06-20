import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { eventApi } from '../api/eventApi';
import { OutputEventBase, OutputEvent } from '../models/event';

const Events: React.FC = () => {
  const [events, setEvents] = useState<OutputEventBase[]>([]);
  const [selectedEvent, setSelectedEvent] = useState<OutputEvent | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [totalPages, setTotalPages] = useState<number>(1);
  const navigate = useNavigate();

  // Загрузка событий
  useEffect(() => {
    const fetchData = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setError('Please log in to access events');
        navigate('/login');
        return;
      }

      try {
        const response = await eventApi.getEventsPaged(currentPage);
        console.log('Paged events response:', response); // Отладка
        console.log('Events array:', response.events);
        console.log('Total count:', response.totalCount);
        console.log('Total pages:', response.totalPages);
        setEvents(Array.isArray(response.events) ? response.events : []);
        setTotalPages(response.totalPages || 1);
        setError(null);
      } catch (err: any) {
        console.error('Error loading events:', err);
        setError(err.response?.data?.title || 'Failed to load events');
        setEvents([]);
      }
    };
    fetchData();
  }, [navigate, currentPage]);

  // Получение деталей события
  const handleGetEvent = async (id: number) => {
    try {
      const event = await eventApi.getEventById(id);
      console.log('Event details:', event); // Отладка
      setSelectedEvent(event);
      setError(null);
    } catch (err: any) {
      console.error('Error getting event:', err);
      setError(err.response?.data?.title || 'Failed to get event');
    }
  };

  // Переключение страниц
  const handlePreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  const handleNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1);
    }
  };

  return (
    <div>
      <h2>События</h2>

      {/* Список событий */}
      <div>
        <h3>Список событий</h3>
        {events.length === 0 && <p>События не найдены</p>}
        <ul>
          {Array.isArray(events) &&
            events.map((event) => (
              <li key={event.id}>
                {event.name} (Организатор: {event.organizer}, Место: {event.location}, Дата: {event.date})
                <button onClick={() => handleGetEvent(event.id)} style={{ marginLeft: '10px' }}>
                  Подробнее
                </button>
              </li>
            ))}
        </ul>
        <div style={{ marginTop: '10px' }}>
          <button onClick={handlePreviousPage} disabled={currentPage === 1}>
            Предыдущая
          </button>
          <span style={{ margin: '0 10px' }}>
            Страница {currentPage} из {totalPages}
          </span>
          <button onClick={handleNextPage} disabled={currentPage === totalPages}>
            Следующая
          </button>
        </div>
      </div>

      {/* Детали выбранного события */}
      {selectedEvent && (
        <div style={{ marginTop: '20px' }}>
          <h3>Детали события</h3>
          <p><strong>ID:</strong> {selectedEvent.id}</p>
          <p><strong>Название:</strong> {selectedEvent.name}</p>
          <p><strong>Организатор:</strong> {selectedEvent.organizer}</p>
          <p><strong>Место:</strong> {selectedEvent.location}</p>
          <p><strong>Дата:</strong> {selectedEvent.date}</p>
        </div>
      )}

      {error && <p style={{ color: 'red' }}>{error}</p>}
    </div>
  );
};

export default Events;