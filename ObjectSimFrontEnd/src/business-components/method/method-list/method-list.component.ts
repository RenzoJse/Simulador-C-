import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';

import { ListComponent } from '../../../components/list/list.component';
import { MethodService } from '../../../backend/services/method/method.service';
import CreateMethod from '../../../backend/services/method/models/method-dto.model';

@Component({
    selector: 'app-method-list',
    templateUrl: './method-list.component.html',
    standalone: true,
    imports: [ListComponent, CommonModule],
})

export class MethodListComponent implements OnInit {

    @Input() methods: CreateMethod[] = [];
    @Input() columns: string[] = ['name', 'type', 'accessibility', 'isSealed', 'isVirtual', 'isOverride', 'isStatic'];
    @Input() tittle: string = 'Methods';

    @Output() selectedMethod = new EventEmitter<CreateMethod>();
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
                    parameters: classItem.parameters || [],
                    returnType: classItem.returnType || '',
                }));
            },
            (error) => {
                console.error('Error loading methods', error);
            }
        )
    }

    selectMethod(methodItem: CreateMethod): void {
        if (methodItem) {
            if (this.actualSelectedMethod === methodItem) {
                this.actualSelectedMethod = undefined;
                this.selectedMethod.emit(undefined);
            } else {
                this.actualSelectedMethod = methodItem;
                this.selectedMethod.emit(methodItem);
            }
        } else {
            this.actualSelectedMethod = undefined;
            this.selectedMethod.emit(undefined);
        }
    }

}
