import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 10,
    duration: '60s',
};

export default function () {
    const url = 'http://localhost:5000/api/user/login';
    const payload = JSON.stringify( {
        "appId": "7de7cf59-9268-4651-b5a5-3d0221a257c6",
        "email": "camarin@camarin.com",
        "password": "Cambiar.100"
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const res = http.post(url, payload, params);
    check(res, { 'status was 200': (r) => r.status == 200 });
}
