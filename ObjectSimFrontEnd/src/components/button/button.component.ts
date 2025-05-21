import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-boton',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './button.component.html',
  styles: ``,
})

export class BotonComponent {
  @Input({ required: true }) title!: string;
  @Input({ required: true }) whenClicked!: () => void;
  @Input() status: string = '';
}
