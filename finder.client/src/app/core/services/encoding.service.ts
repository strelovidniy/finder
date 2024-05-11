import { Injectable } from '@angular/core';


@Injectable({
    providedIn: 'root'
})
export default class EncodingService {
    public encode<T>(json: T): string {
        const jsonString = JSON.stringify(json);
        const byteArray = new TextEncoder().encode(jsonString);
        const byteArrayNumbers = Array.from(byteArray);


        return btoa(String.fromCharCode.apply(null, byteArrayNumbers));
    }

    public decode<T>(base64: string): T {
        const decodedByteArray = new Uint8Array(atob(base64).split('').map((char: string): number => char.charCodeAt(0)));
        const decodedJsonString = new TextDecoder().decode(decodedByteArray);


        return JSON.parse(decodedJsonString);
    }
}
