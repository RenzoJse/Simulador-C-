import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';

import { ListComponent } from '../../../components/list/list.component';
import { MethodService } from '../../../backend/services/method/method.service';
import CreateMethod from '../../../backend/services/method/models/method-dto.model';
import {MethodListItem} from "../../../backend/services/method/models/method-list-item.model";
import MethodDTO from "../../../backend/services/method/models/method-dto.model";

@Component({
    selector: 'app-method-list',
    templateUrl: './method-list.component.html',
    standalone: true,
    imports: [ListComponent, CommonModule],
})

export class MethodListComponent implements OnInit {

    @Input() methods: MethodWithIds[] = [];
    @Input() columns: string[] = ['name', 'type', 'accessibility', 'isSealed', 'isVirtual', 'isOverride', 'isStatic', 'id'];
    @Input() tittle: string = 'Methods';

    @Output() selectedMethod = new EventEmitter<string | undefined>();
    actualSelectedMethod?: any;

    constructor(private _methodService: MethodService) {
    }

    ngOnInit(): void {
        this.loadMethods();
    }

    private loadMethods(): void {
        this._methodService.getAllMethods().subscribe(
            (data) => {
                this.methods = data.map((classItem: any) => ({
                    ...classItem,
                    id: classItem.id,
                    classId: classItem.classId ?? '',
                    // el resto de las propiedades...
                }));
            },
            (error) => {
                console.error('Error loading methods', error);
            }
        )
    }

    selectMethod(methodId: string | undefined): void {
        if (!methodId) {
            this.actualSelectedMethod = undefined;
            this.selectedMethod.emit(undefined);
            return;
        }

        const method = this.methods.find(m => m.id === methodId);
        if (this.actualSelectedMethod === method) {
            this.actualSelectedMethod = undefined;
            this.selectedMethod.emit(undefined);
        } else {
            this.actualSelectedMethod = method;
            this.selectedMethod.emit(method?.id);
        }
    }
}

interface MethodWithIds extends CreateMethod {
    id: string;
    classId: string;
}