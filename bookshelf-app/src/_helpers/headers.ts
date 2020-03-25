export function getHeaders() {
    // return authorization header with jwt token
    const val = localStorage.getItem('user');
    const user = JSON.parse(val!);
 
    if (user && user.token) {
        return { 'Authorization': 'Bearer ' + user.token };
    }
}