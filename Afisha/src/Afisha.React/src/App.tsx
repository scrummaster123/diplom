import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { RootState, AppDispatch } from './store';
import { setMessage } from './store/messageSlice';

const App: React.FC = () => {
  const message = useSelector((state: RootState) => state.message.text);
  const dispatch = useDispatch<AppDispatch>();

  const handleChangeMessage = () => {
    dispatch(setMessage('Hello from Redux!'));
  };

  return (
    <div>
      <h1>{message}</h1>
      <button onClick={handleChangeMessage}>Change Message</button>
    </div>
  );
};

export default App;