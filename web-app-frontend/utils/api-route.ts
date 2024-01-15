export class ApiRoutes {
    static apiUrl = 'http://72.167.140.189:5289';
    static login = `${ApiRoutes.apiUrl}/api/auth/login`;
    static register = `${ApiRoutes.apiUrl}/api/auth/register`;
    static dashboard = `${ApiRoutes.apiUrl}/api/Dashboard/GetDashboard`;
    static loginByGoogle = `${ApiRoutes.apiUrl}/api/auth/google`;
    static checkTeamName = `${ApiRoutes.apiUrl}/api/auth/`;
  }
  