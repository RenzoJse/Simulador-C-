import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';

import { ListComponent } from '../../../components/list/list.component';
import { ClassService } from '../../../backend/services/class/class.service';
import CreateClassModel from '../../../backend/services/class/models/create-class.model';

@Component({
    selector: 'app-class-list',
    templateUrl: './class-list.component.html',
    standalone: true,
    imports: [ListComponent, CommonModule],
})

export class ClassListComponent implements OnInit {

@Input() classes: CreateClassModel[] = [];
@Input() columns: string[] = ['name', 'accessibility', 'isAbstract', 'isSealed', 'isVirtual', 'methods', 'attributes', 'parent'];
@Input() tittle: string = 'Classes';

@Output() selectedClass = new EventEmitter<CreateClassModel>();

actualSelectedClass?: any;
constructor(private _classService: ClassService) {
}

ngOnInit(): void {
    this.loadClasses();
}

private loadClasses(): void {
    this._classService.getAllClasses().subscribe(
        (data) => {
            this.classes = data.map((classItem: any) => ({
                ...classItem,
                methods: classItem.methods || [],
                attributes: classItem.attributes || [],
                parent: classItem.parent || '',
            }));
        },
        (error) => {
            console.error('Error loading classes', error);
        }
    )
}

selectClass(classItem: CreateClassModel): void {
    if (classItem) {
        if (this.actualSelectedClass === classItem) {
            this.actualSelectedClass = undefined;
            this.selectedClass.emit(undefined);
        } else {
            this.actualSelectedClass = classItem;
            this.selectedClass.emit(classItem);
        }
    } else {
        this.actualSelectedClass = undefined;
        this.selectedClass.emit(undefined);
    }
}

}
