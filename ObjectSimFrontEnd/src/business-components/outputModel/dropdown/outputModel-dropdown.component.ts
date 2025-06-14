import { Component, Input, OnDestroy, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from "rxjs";

import { OutputModelService } from '../../../backend/services/outputModel/outputModel.service';
import { DropdownComponent } from '../../../components/dropdown/dropdown.component';
import SystemOutputModelsStatus from './models/SystemOutputModelsStatus';

@Component({
    selector: 'app-outputModel-dropdown',
    standalone: true,
    imports: [DropdownComponent, CommonModule],
    templateUrl: './outputModel-dropdown.component.html',
})

export class OutputModelDropdownComponent implements OnInit, OnDestroy{
    @Input() value: string | null = null;
    @Output() selectModel = new EventEmitter<{ name: string | undefined; }>();

    status: SystemOutputModelsStatus = {
        loading: true,
        systemModels: [],
        error: '',
    };

    private _everyoneStatus: Subscription | null = null;

    constructor(private readonly _outputModelService: OutputModelService) {}
    ngOnDestroy(): void {
        this._everyoneStatus?.unsubscribe();
    }

    ngOnInit(): void {
        this._outputModelService.getImplementationList()
            .subscribe({
                next: (systemModels) => {
                    this.status = {
                        systemModels: systemModels.map((model) => ({
                            value: model.name,
                            tag: model.name,
                        })),
                    };
                },
                error: (error) => {
                    this.status = { systemModels: [], error: 'No available output models.' };
                }
            });
    }

    onSelectModel(modelName: string) {
        const model = this.status.systemModels.find(c => c.value === modelName);
        if (model) {
            this.selectModel.emit({
                name: model.value,
            });
        }
    }
}
