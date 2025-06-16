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
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.router.navigate(['invokemethod'], { relativeTo: this.route });
        }
    }
}