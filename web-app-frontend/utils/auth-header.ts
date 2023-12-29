export function authHeader(): Record<string, string> {
    const userString = localStorage.getItem('user');
    let headers: Record<string, string> = {};
  
    if (userString) {
      const user = JSON.parse(userString);
      if (user && user.token) {
        headers = { 'Authorization': 'Bearer ' + user.token };
      }
    }
  
    return headers;
  }
  