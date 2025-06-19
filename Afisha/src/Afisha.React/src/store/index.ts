import { configureStore } from '@reduxjs/toolkit';
import messageReducer from './messageSlice';
import authReducer from './authSlice';

export const store = configureStore({
  reducer: {
    message: messageReducer,
    auth: authReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;