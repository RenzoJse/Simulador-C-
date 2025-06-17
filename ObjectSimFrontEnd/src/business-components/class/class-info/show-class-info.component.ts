import {Component, Input, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';

import {ActivatedRoute} from "@angular/router";
import {InfoCardComponent} from "../../../components/info-card/info-card.component";

import {ClassService} from "../../../backend/services/class/class.service";
import ClassDtoOut from "../../../backend/services/class/models/class-dto-out";

@Component({
    selector: 'app-show-class-info',
    templateUrl: './show-class-info.component.html',
    standalone: true,
    imports: [CommonModule, InfoCardComponent],
    styleUrls: ['./show-class-info.component.css']
})

export class ShowClassInfoComponent implements OnInit {

    @Input() classes: ClassDtoOut[] = [];
    @Input() columns: string[] = ['name', 'accessibility', 'isAbstract', 'isSealed', 'isVirtual', 'Methods', 'Attributes', 'isStatic', 'id'];

    classInfo!: ClassDtoOut;
    public classInfoData: any[] = [];

    constructor(
        private _classService: ClassService,
        private route: ActivatedRoute
    ) {}

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            const classId = params.get('classId');
            if (classId) {
                this.loadMethodInfo(classId);
            }
        });
    }

    showOrNot() {
        if (this.classInfo) {
            this.classInfoData = [
                { label: 'Name', value: this.classInfo.name },
                { label: 'Parent', value: this.classInfo.parent },
                ...(this.classInfo.isAbstract ? [{ label: 'Is Abstract', value: 'Yes' }] : []),
                ...(this.classInfo.isSealed ? [{ label: 'Is Sealed', value: 'Yes' }] : []),
                ...(this.classInfo.isVirtual ? [{ label: 'Is Virtual', value: 'Yes' }] : []),
                ...(this.classInfo.isInterface ? [{ label: 'Is Interface', value: 'Yes' }] : []),
                ...(this.classInfo.methods?.length
                    ? [{ label: 'Methods', value: this.classInfo.methods.map(p => p.name ?? JSON.stringify(p)).join(', ') }]
                    : []),
                ...(this.classInfo.attributes?.length
                    ? [{ label: 'Attributes', value: this.classInfo.attributes.map(p => p.name ?? JSON.stringify(p)).join(', ') }]
                    : []),
            ];
        }
    }

    private loadMethodInfo(classId: string): void {
        this._classService.getById(classId).subscribe({
            next: (data) => {
                if (data) {
                    this.classInfo = Array.isArray(data) ? data[0] : data as ClassDtoOut;
                    this.showOrNot();
                } else {
                    this.classInfo = undefined!;
                    this.classInfoData = [];
                }
            },
            error: (error) => {
                console.error('Error loading class info', error);
            }
        });
    }
}