import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { locationApi } from '../api/locationApi';
import { OutputLocationBase, OutputLocationFull } from '../models/location';

const Locations: React.FC = () => {
  const [locations, setLocations] = useState<OutputLocationBase[]>([]);
  const [selectedLocation, setSelectedLocation] = useState<OutputLocationFull | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [totalPages, setTotalPages] = useState<number>(1);
  const navigate = useNavigate();

  // Загрузка локаций
  useEffect(() => {
    const fetchData = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setError('Please log in to access locations');
        navigate('/login');
        return;
      }

      try {
        const response = await locationApi.getLocationsPaged(currentPage);
        console.log('Paged locations response:', response); // Отладка
        setLocations(Array.isArray(response.locations) ? response.locations : []);
        setTotalPages(response.totalPages || 1);
        setError(null);
      } catch (err: any) {
        console.error('Error loading locations:', err);
        setError(err.response?.data?.title || 'Failed to load locations');
        setLocations([]);
      }
    };
    fetchData();
  }, [navigate, currentPage]);

  // Получение деталей локации
  const handleGetLocation = async (id: number) => {
    try {
      const location = await locationApi.getLocationById(id);
      setSelectedLocation(location);
      setError(null);
    } catch (err: any) {
      console.error('Error getting location:', err);
      setError(err.response?.data?.title || 'Failed to get location');
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
      <h2>Локации</h2>

      {/* Список локаций */}
      <div>
        <h3>Список локаций</h3>
        {locations.length === 0 && <p>Локации не найдены</p>}
        <ul>
          {Array.isArray(locations) &&
            locations.map((location) => (
              <li key={location.ownerId}>
                {location.name} (Цена: {location.pricing}, {location.isWarmPlace ? 'Помещение' : 'Открытая зона'})
                <button onClick={() => handleGetLocation(location.ownerId)} style={{ marginLeft: '10px' }}>
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

      {/* Детали выбранной локации */}
      {selectedLocation && (
        <div style={{ marginTop: '20px' }}>
          <h3>Детали локации</h3>
          <p><strong>Owner ID:</strong> {selectedLocation.ownerId}</p>
          <p><strong>Название:</strong> {selectedLocation.name}</p>
          <p><strong>Цена:</strong> {selectedLocation.pricing}</p>
          <p><strong>Тип:</strong> {selectedLocation.isWarmPlace ? 'Помещение' : 'Открытая зона'}</p>
          <p><strong>События:</strong> {selectedLocation.events.length > 0 ? selectedLocation.events.join(', ') : 'Нет событий'}</p>
        </div>
      )}

      {error && <p style={{ color: 'red' }}>{error}</p>}
    </div>
  );
};

export default Locations;