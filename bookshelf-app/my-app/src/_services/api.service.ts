import { getHeaders } from "../_helpers/headers";
import { config } from "../_helpers/config";
import { Book } from "../_models/Book";

export async function login(username: string, password: string) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    };
 
    const response = await fetch('https://localhost:44347/Users/authenticate', requestOptions);
    const user = await handleResponse(response);
    // login successful if there's a jwt token in the response
    if (user && (user as any).token) {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem('user', JSON.stringify(user));
    }
    return user;
}

export async function register(username: string, password: string ){
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    };
 
    const response = await fetch(config.apiUrl + '/Users/Register', requestOptions);
    const user = await handleResponse(response);
    return user;
}

export async function getAllBooks() {
    const requestOptions = {
        method: 'GET',
        headers: getHeaders()
    };
 
    return fetch(config.apiUrl + '/books', requestOptions).then(handleResponse);
}

export async function loanBook(book: Book) {
    const requestOptions = {
        method: 'POST',
        headers: getHeaders()
    };
 
    return fetch(config.apiUrl + '/loan/' + book.id, requestOptions).then(handleResponse);
}

function handleResponse(response : Response) {
    return new Promise((resolve, reject) => {
        if (response.ok) {
            // return json if it was returned in the response
            var contentType = response.headers.get("content-type");
            if (contentType && contentType.includes("application/json")) {
                response.json().then(json => resolve(json));
            } else {
                resolve();
            }
        } else {
            // return error message from response body
            response.text().then(text => reject(text));
        }
    });
}

export function isAuthenticated() {
    return !!localStorage.getItem('user');
}
 