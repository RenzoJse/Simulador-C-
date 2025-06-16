import { Component, Input } from '@angular/core';
import {CommonModule, NgIf} from "@angular/common";

@Component({
    selector: 'app-info-card',
    imports: [CommonModule, NgIf],
    templateUrl: './info-card.component.html',
    styleUrls: ['./info-card.component.css'],
    standalone: true
})

export class InfoCardComponent {
    @Input() title: string = '';
    @Input() data: { label: string, value: string }[] = [];
    @Input() footerText?: string;
}