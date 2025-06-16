import { Component } from '@angular/core';

@Component({
    selector: 'app-method-info',
    templateUrl: './method-info.component.html',
})

export class MethodInfoComponent {
    status: { loading?: true; error?: string } | null = null;
}