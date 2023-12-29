import create from 'zustand';
import { JwtPayload } from 'jwt-decode';
import { UserService } from 'models/UserService';
import { authHeader } from './auth-header';
import { ApiRoutes } from './api-route';

export const useUserStore = create<UserService>((set) => ({
  user: null,
  isLoading: false,
  error: null,
  errors: null,
  success: false,
  data: null,
  team: null,

  fetchUserProfile: (accessToken: string): JwtPayload | null => {
    // Implement the logic for fetching user profile using the accessToken
    return null;
  },

  LoginByGoogle: async (teamid: string, username: string) => {
    set({ isLoading: true });

    const requestBody = {
      Email: username,
    };

    try {
      const response = await fetch(ApiRoutes.loginByGoogle, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody),
      });

      if (!response.ok) {
        throw new Error('Google login failed');
      }

      const user = await response.json();

      if (user && user.token) {
        localStorage.setItem('user', JSON.stringify(user));
        set({ user, isLoading: false, error: null });
      }

      return user;
    } catch (error) {
      set({ isLoading: false, error: error.message || 'Google login failed' });
      throw error;
    }
  },

  login: async (teamid: string, username: string, password: string) => {
    set({ isLoading: true });

    const requestBody = {
      Email: username,
      Password: password,
    };

    try {
      const response = await fetch(ApiRoutes.login, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody),
      });

      if (!response.ok) {
        throw new Error('Login failed');
      }

      const user = await response.json();

      if (user && user.token) {
        localStorage.setItem('user', JSON.stringify(user));
        set({ user, isLoading: false, error: null });
      }

      return user;
    } catch (error) {
      set({ isLoading: false, error: error.message || 'Login failed' });
      throw error;
    }
  },

  register: async (teamname: string, email: string, password: string) => {
    set({ isLoading: true });
  
    const requestBody = {
      Email: email,
      Password: password,
      UserName: teamname,
    };
  
    try {
      const response = await fetch(ApiRoutes.register, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody),
      });
  
      if (!response.ok) {
        const errorResponse = await response.json();
        return errorResponse;
      }
  
      const responseData = await response.json();
      return responseData;
    } catch (error: any) {
      throw new Error('Registration failed');
    }
  },
  

  checkTeamName: async (teamName: string) => {
    set({ isLoading: true });

    try {
      const response = await fetch(ApiRoutes.checkTeamName + teamName, {
        method: 'GET',
        headers: authHeader(), // Define or import authHeader function
      });

      if (!response.ok) {
        throw new Error('Team name check failed');
      }

      set({ isLoading: false, error: null });
      return response.json();
    } catch (error) {
      set({ isLoading: false, error: 'Team name check failed' });
      throw error;
    }
  },

  logout: () => {
    localStorage.removeItem('user');
    set({ user: null, error: null });
  },
}));
