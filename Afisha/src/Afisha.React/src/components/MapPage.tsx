import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { locationMapApi } from '../api/locationMapApi';
import { eventApi } from '../api/eventApi';
import { authApi } from '../api/authApi';
import { OutputLocation } from '../models/event';

const MapPage: React.FC = () => {
  const [map, setMap] = useState<{ mapUrl: string; mapWidth: number; mapHeight: number } | null>(null);
  const [locations, setLocations] = useState<OutputLocation[]>([]); // Initialize as empty array
  const [selectedLocation, setSelectedLocation] = useState<OutputLocation | null>(null);
  const [eventForm, setEventForm] = useState({ name: '', organizer: '', date: '' });
  const [error, setError] = useState<string | null>(null);
  const [userId, setUserId] = useState<number | null>(null);
  const navigate = useNavigate();

  // Load userId, map, and locations
  useEffect(() => {
    const fetchData = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setError('Please log in to access the map');
        navigate('/login');
        return;
      }

      try {
        const user = await authApi.getCurrentUser();
        console.log('Fetched User ID:', user.userId);
        setUserId(user.userId);
      } catch (err: any) {
        console.error('Failed to fetch user ID:', err);
        setError(err.response?.data?.title || 'Failed to fetch user ID');
        navigate('/login');
        return;
      }

      try {
        const mapData = await locationMapApi.getMap();
        console.log('Map data:', mapData);
        setMap(mapData);

        const locationsData = await locationMapApi.getLocations();
        console.log('Locations data:', locationsData);
        setLocations(Array.isArray(locationsData) ? locationsData : []); // Ensure array
        setError(null);
      } catch (err: any) {
        console.error('Error loading map or locations:', err);
        const errorMessage =
          err.response?.data?.details ||
          err.response?.data?.error ||
          err.response?.data?.title ||
          'Failed to load map or locations';
        console.log('Error response:', err.response?.data);
        setError(errorMessage);
      }
    };
    fetchData();
  }, [navigate]);

  // Handle location click
  const handleLocationClick = (location: OutputLocation) => {
    setSelectedLocation(location);
    setEventForm({ name: '', organizer: '', date: '' });
    setError(null);
  };

  // Handle form submission
  const handleCreateEvent = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedLocation || !userId) {
      setError('Please select a location and ensure you are logged in');
      return;
    }

    try {
      await eventApi.createEvent({
        name: eventForm.name,
        organizer: eventForm.organizer,
        locationId: selectedLocation.id,
        dateStart: eventForm.date,
      });
      setError(null);
      setSelectedLocation(null);
      setEventForm({ name: '', organizer: '', date: '' });
      navigate('/events');
    } catch (err: any) {
      console.error('Error creating event:', err);
      const errorMessage =
        err.response?.data?.details ||
        err.response?.data?.error ||
        err.response?.data?.title ||
        'Failed to create event';
      console.log('Error response:', err.response?.data);
      setError(errorMessage);
    }
  };

  return (
    <div>
      <h2>Карта локаций</h2>

      {/* Map and locations */}
      {map ? (
        <div style={{ position: 'relative', width: map.mapWidth, height: map.mapHeight }}>
          <img
            src={map.mapUrl}
            alt="Map"
            style={{ width: '100%', height: '100%', objectFit: 'contain' }}
            onError={() => setError('Failed to load map image')}
          />
          {Array.isArray(locations) && locations.length > 0 ? (
            locations.map((location) => (
              <button
                key={location.id} // Ensure unique keys
                style={{
                  position: 'absolute',
                  left: location.xCoordinate,
                  top: location.yCoordinate,
                  width: 20,
                  height: 20,
                  background: 'red',
                  borderRadius: '50%',
                  border: 'none',
                  cursor: 'pointer',
                  transform: 'translate(-50%, -50%)',
                }}
                title={location.name}
                onClick={() => handleLocationClick(location)}
              />
            ))
          ) : (
            <p>No locations available</p>
          )}
        </div>
      ) : (
        <p>Loading map...</p>
      )}

      {/* Event creation form */}
      {selectedLocation && (
        <div style={{ marginTop: '20px' }}>
          <h3>Создать событие в локации: {selectedLocation.name}</h3>
          <form onSubmit={handleCreateEvent}>
            <div>
              <label>Название события:</label>
              <input
                type="text"
                value={eventForm.name}
                onChange={(e) => setEventForm({ ...eventForm, name: e.target.value })}
                required
              />
            </div>
            <div>
              <label>Организатор:</label>
              <input
                type="text"
                value={eventForm.organizer}
                onChange={(e) => setEventForm({ ...eventForm, organizer: e.target.value })}
                required
              />
            </div>
            <div>
              <label>Дата (YYYY-MM-DD):</label>
              <input
                type="text"
                value={eventForm.date}
                onChange={(e) => setEventForm({ ...eventForm, date: e.target.value })}
                required
                placeholder="2025-06-28"
              />
            </div>
            <button type="submit">Создать событие</button>
            <button
              type="button"
              onClick={() => setSelectedLocation(null)}
              style={{ marginLeft: '10px' }}
            >
              Отмена
            </button>
          </form>
        </div>
      )}

      {error && <p style={{ color: 'red' }}>{error}</p>}
    </div>
  );
};

export default MapPage;