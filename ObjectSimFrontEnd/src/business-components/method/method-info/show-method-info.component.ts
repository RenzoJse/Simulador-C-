import { Component, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MethodService } from '../../../backend/services/method/method.service';
import CreateMethod from '../../../backend/services/method/models/method-dto.model';
import { ActivatedRoute } from "@angular/router";
import {InfoCardComponent} from "../../../components/info-card/info-card.component";
import MethodDTO from "../../../backend/services/method/models/method-dto.model";

@Component({
    selector: 'app-show-method-info',
    templateUrl: './show-method-info.component.html',
    standalone: true,
    imports: [CommonModule, InfoCardComponent],
    styleUrls: ['./show-method-info.component.css']
})

export class ShowMethodInfoComponent implements OnInit {

    @Input() methods: MethodDTOWithId[] = [];
    @Input() columns: string[] = ['name', 'type', 'classId', 'accessibility', 'isSealed', 'isVirtual', 'isOverride', 'isStatic', 'id'];

    methodInfo!: MethodDTOWithId;
    public methodInfoData: any[] = [];

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

    showOrNot() {
        if (this.methodInfo) {
            console.log('methodInfo:', this.methodInfo);
            this.methodInfoData = [
                { label: 'Name', value: this.methodInfo.name },
                { label: 'Type', value: this.methodInfo.type },
                { label: 'Class Id', value: this.methodInfo.classId },
                { label: 'Accessibility', value: this.getAccessibilityText(this.methodInfo.accessibility) },
                ...(this.methodInfo.isAbstract ? [{ label: 'Is Abstract', value: 'Yes' }] : []),
                ...(this.methodInfo.isSealed ? [{ label: 'Is Sealed', value: 'Yes' }] : []),
                ...(this.methodInfo.isOverride ? [{ label: 'Is Override', value: 'Yes' }] : []),
                ...(this.methodInfo.isVirtual ? [{ label: 'Is Virtual', value: 'Yes' }] : []),
                ...(this.methodInfo.isStatic ? [{ label: 'Is Static', value: 'Yes' }] : []),
                ...(this.methodInfo.parameters?.length
                    ? [{ label: 'Parameters', value: this.methodInfo.parameters.map(p => p.name ?? JSON.stringify(p)).join(', ') }]
                    : []),
                ...(this.methodInfo.localVariables?.length
                    ? [{ label: 'Local Variables', value: this.methodInfo.localVariables.map(v => v.name ?? JSON.stringify(v)).join(', ') }]
                    : []),
                ...(this.methodInfo.invokeMethodsIds?.length
                    ? [{ label: 'Invoked Method IDs', value: this.methodInfo.invokeMethodsIds.join(', ') }]
                    : []),
            ];
        }
    }

    getAccessibilityText(value: any): string {
        switch (value) {
            case 0: return 'Public';
            case 1: return 'Private';
            case 2: return 'Protected';
            default: return value?.toString() ?? '';
        }
    }

    private loadMethodInfo(methodId: string): void {
        console.log('Load MethodInfo ' + methodId);
        this._methodService.getMethodById(methodId).subscribe({
            next: (data) => {
                if (data) {
                    console.log(data);
                    this.methods = [{
                        ...data,
                        id: data.id,
                        classId: data.classId ?? '',
                    }];
                    this.methodInfo = {
                        name: data.name,
                        type: data.type,
                        classId: data.classId,
                        accessibility: data.accessibility,
                        isAbstract: data.isAbstract,
                        isSealed: data.isSealed,
                        isOverride: data.isOverride,
                        isVirtual: data.isVirtual,
                        isStatic: data.isStatic,
                        localVariables: data.localVariables ?? [],
                        parameters: data.parameters ?? [],
                        invokeMethodsIds: data.invokeMethodsIds ?? [],
                        id: data.id,
                    };
                    this.showOrNot();
                } else {
                    this.methods = [];
                    this.methodInfo = undefined!;
                    this.methodInfoData = [];
                }
            },
            error: (error) => {
                console.error('Error loading methods', error);
            }
        });
    }
}

export interface MethodDTOWithId extends MethodDTO {
    id: string;
    invokeMethodsIds?: string[];
}