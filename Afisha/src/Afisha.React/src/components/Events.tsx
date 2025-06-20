import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { eventApi } from '../api/eventApi';
import { authApi } from '../api/authApi';
import { OutputEventBase } from '../models/event';

const Events: React.FC = () => {
  const [events, setEvents] = useState<OutputEventBase[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [totalPages, setTotalPages] = useState<number>(1);
  const [userId, setUserId] = useState<number | null>(null);
  const navigate = useNavigate();

  // Загрузка событий
  const fetchEvents = async () => {
    try {
      const response = await eventApi.getEventsPaged(currentPage);
      console.log('Paged events response:', response);
      console.log('Events array:', response.events);
      console.log('Total count:', response.totalCount);
      console.log('Total pages:', response.totalPages);
      response.events.forEach((event, index) => {
        console.log(`Event ${index + 1} participants:`, event.participants);
        console.log(`Is userId ${userId} in participants?`, userId && event.participants?.includes(userId));
      });
      setEvents(Array.isArray(response.events) ? response.events : []);
      setTotalPages(response.totalPages || 1);
      setError(null);
    } catch (err: any) {
      console.error('Error loading events:', err);
      setError(err.response?.data?.title || 'Failed to load events');
      setEvents([]);
    }
  };

  // Загрузка UserId и событий
  useEffect(() => {
    const fetchData = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setError('Please log in to access events');
        navigate('/login');
        return;
      }

      // Получаем UserId
      try {
        const user = await authApi.getCurrentUser();
        console.log('Fetched User ID:', user.userId);
        setUserId(user.userId);
      } catch (err) {
        console.error('Failed to fetch user ID:', err);
        setError('Failed to fetch user ID');
        navigate('/login');
        return;
      }

      await fetchEvents();
    };
    fetchData();
  }, [navigate, currentPage]);

  // Присоединение к событию
  const handleJoinEvent = async (eventId: number) => {
    if (userId === null) {
      setError('User ID is not available');
      navigate('/login');
      return;
    }
    try {
      await eventApi.joinEvent(eventId, userId);
      console.log(`Joined event ${eventId} with userId ${userId}`);
      setError(null);
      await fetchEvents(); // Обновляем список событий
    } catch (err: any) {
      console.error('Error joining event:', err);
      const errorMessage =
        err.response?.data?.details ||
        err.response?.data?.error ||
        err.response?.data?.title ||
        'Failed to join event';
      console.log('Error response data:', err.response?.data); // Debug log
      setError(errorMessage);
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
                {/* Проверяем participants на null/undefined */}
                {userId !== null && (!event.participants || !event.participants.includes(userId)) ? (
                  <button onClick={() => handleJoinEvent(event.id)} style={{ marginLeft: '10px' }}>
                    Присоединиться
                  </button>
                ) : (
                  <span style={{ marginLeft: '10px' }}>(Вы участвуете)</span>
                )}
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

      {error && <p style={{ color: 'red' }}>{error}</p>}
    </div>
  );
};

export default Events;