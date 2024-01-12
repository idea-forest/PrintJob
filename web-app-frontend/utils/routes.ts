export class DashRoutes {
  static home = '/';
  static login = '/';
  static register = '/auth/register';
  static dashboard = '/dashboard';
  static device = '/device';

  static settings = `${this.dashboard}/settings`;
  static deviceList = `${this.device}/list`;
}
