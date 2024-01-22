export class ApiRoutes {
    static apiUrl = 'https://api.printbloc.com';
    static login = `${ApiRoutes.apiUrl}/api/auth/login`;
    static register = `${ApiRoutes.apiUrl}/api/auth/register`;
    static dashboard = `${ApiRoutes.apiUrl}/api/Dashboard/GetDashboard`;
    static loginByGoogle = `${ApiRoutes.apiUrl}/api/auth/google`;
    static checkTeamName = `${ApiRoutes.apiUrl}/api/auth/`;
    static uploadFile = `${ApiRoutes.apiUrl}/uploadFile`
  }
  