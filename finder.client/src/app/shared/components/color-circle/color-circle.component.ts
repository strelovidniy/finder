import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ColorPickerControl } from '@iplab/ngx-color-picker';


@Component({
    selector: 'finder-color-circle',
    templateUrl: './color-circle.component.html',
    styleUrls: ['./color-circle.component.scss']
})
export default class ColorCircleComponent implements OnInit {
    @Input() public color: string = 'transparent';
    @Input() public selected: boolean = false;
    @Input() public customColor: boolean = false;
    @Output() public click = new EventEmitter<void>();
    @Output() public customColorChange = new EventEmitter<string>();

    public pickerControl: ColorPickerControl = new ColorPickerControl();

    public ngOnInit(): void {
        this.pickerControl.hideAlphaChannel()
            .hidePresets();
    }

    public onClick(): void {
        this.click.emit();
    }

    public setCustomColor(color: string): void {
        this.color = color;

        this.customColorChange.emit(this.color);
    }
}
