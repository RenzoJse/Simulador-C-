import { Component, Input } from '@angular/core';
import { CommonModule, NgIf, NgFor } from '@angular/common';

@Component({
    selector: 'app-info-guide',
    standalone: true,
    imports: [CommonModule, NgIf, NgFor],
    templateUrl: './info-guide.component.html',
    styleUrls: ['./info-guide.component.css']
})
export class InfoGuideComponent {
    @Input() guides: { id: string, title: string, description: string }[] = [];
    @Input() footerText?: string;
}