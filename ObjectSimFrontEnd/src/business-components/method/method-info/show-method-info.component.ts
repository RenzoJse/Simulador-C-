import { Component, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ListComponent } from '../../../components/list/list.component';
import { MethodService } from '../../../backend/services/method/method.service';
import CreateMethod from '../../../backend/services/method/models/method-dto.model';
import { ActivatedRoute } from "@angular/router";
import {InfoCardComponent} from "../../../components/info-card/info-card.component";

@Component({
    selector: 'app-show-method-info',
    templateUrl: './show-method-info.component.html',
    standalone: true,
    imports: [ListComponent, CommonModule, InfoCardComponent],
    styleUrls: ['./show-method-info.component.css']
})

export class ShowMethodInfoComponent implements OnInit {

    @Input() methods: MethodWithIds[] = [];
    @Input() columns: string[] = ['name', 'type', 'accessibility', 'isSealed', 'isVirtual', 'isOverride', 'isStatic', 'id'];

    methodInfo!: {
        name: string;
        type: string;
        accessibility: string;
        isVirtual: boolean;
        isStatic: boolean;
        id?: string;
    };

    constructor(
        private _methodService: MethodService,
        private route: ActivatedRoute
    ) {}

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            const methodId = params.get('methodId');
            if (methodId) {
                this.loadMethodInfo(methodId);
            }
        });
    }

    private loadMethodInfo(methodId: string): void {
        console.log('Load MethodInfo ' + methodId);
        this._methodService.getMethodById(methodId).subscribe({
            next: (data) => {
                if (data) {
                    this.methods = [{
                        ...data,
                        id: data.id,
                        classId: data.classId ?? '',
                    }];
                    this.methodInfo = {
                        name: data.name,
                        type: data.type,
                        accessibility: data.accessibility,
                        isVirtual: data.isVirtual,
                        isStatic: data.isStatic,
                        id: data.id,
                    };
                } else {
                    this.methods = [];
                    this.methodInfo = undefined!;
                }
            },
            error: (error) => {
                console.error('Error loading methods', error);
            }
        });
    }
}

interface MethodWithIds extends CreateMethod {
    id: string;
    classId: string;
}