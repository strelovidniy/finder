import { Injectable } from '@angular/core';


@Injectable({
    providedIn: 'root'
})
export default class ImageService {
    public get defaultImageUrl(): string {
        return 'https://www.vocaleurope.eu/wp-content/uploads/no-image.jpg';
    }
}
