export class DashRoutes {
  static home = '/';
  static login = '/';
  static register = '/auth/register';
  static dashboard = '/dashboard';
  static device = '/device';
  static printJobs = '/printjobs';

  static settings = `${this.dashboard}/settings`;
  static deviceList = `${this.device}/list`;
  static printJobsList = `${this.printJobs}/list`;
}
