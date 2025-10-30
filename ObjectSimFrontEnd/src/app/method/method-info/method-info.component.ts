import { Component } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";

@Component({
    selector: 'app-method-info',
    templateUrl: './method-info.component.html',
})

export class MethodInfoComponent {
    status: { loading?: true; error?: string } | null = null;

    constructor(private router: Router, private route: ActivatedRoute) {}

    goToAddInvokeMethod() {
        this.router.navigate(['invoke-method'], { relativeTo: this.route });
    }
}