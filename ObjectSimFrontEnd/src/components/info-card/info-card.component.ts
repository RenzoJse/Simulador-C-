import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-info-card',
    templateUrl: './info-card.component.html',
    styleUrls: ['./info-card.component.css'],
    standalone: true
})

export class InfoCardComponent {
    @Input() title: string = '';
    @Input() data: { label: string, value: string }[] = [];
    @Input() footerText?: string;
}