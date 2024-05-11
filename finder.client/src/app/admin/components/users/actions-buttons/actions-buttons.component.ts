import { Component,EventEmitter, Output } from '@angular/core';


@Component({
    selector: 'finder-actions-buttons',
    templateUrl: './actions-buttons.component.html',
    styleUrls: ['./actions-buttons.component.scss']
})
export default class ActionsButtonsComponent {

  @Output() private delete = new EventEmitter<void>();
  @Output() private edit = new EventEmitter<void>();


  public onDeleteClick(): void {
      this.delete.emit();
  }

  public onEditClick(): void {
      this.edit.emit();
  }
}
