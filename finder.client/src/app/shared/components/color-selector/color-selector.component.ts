import { Component, EventEmitter, Input, Output } from '@angular/core';


@Component({
    selector: 'finder-color-selector',
    templateUrl: './color-selector.component.html',
    styleUrls: ['./color-selector.component.scss']
})
export default class ColorSelectorComponent {

    @Input() public allowCustom: boolean = false;
    @Input() public colors: string[] = [];
    @Input() public selectedColor: string = '';
    @Output() public selectedColorChange = new EventEmitter<string>();

    public selectColor(color: string): void {
        this.selectedColor = color;

        this.selectedColorChange.emit(color);
    }
}
