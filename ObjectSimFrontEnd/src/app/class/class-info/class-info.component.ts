import { Component } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";

@Component({
    selector: 'app-class-info',
    templateUrl: './class-info.component.html',
})

export class ClassInfoComponent {
    status: { loading?: true; error?: string } | null = null;

    constructor() {}
}