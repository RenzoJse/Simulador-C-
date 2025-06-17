import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';

import { ListComponent } from '../../../components/list/list.component';
import { ClassService } from '../../../backend/services/class/class.service';

import ClassDtoOut from "../../../backend/services/class/models/class-dto-out";

@Component({
    selector: 'app-class-list',
    templateUrl: './class-list.component.html',
    standalone: true,
    imports: [ListComponent, CommonModule],
})

export class ClassListComponent implements OnInit {

    @Input() classes: ClassDtoOut[] = [];
    @Input() columns: string[] = ['name', 'accessibility', 'isAbstract', 'isSealed', 'isVirtual', 'methods', 'attributes', 'parent'];
    @Input() tittle: string = 'Classes';

    @Output() selectedClass = new EventEmitter<string | undefined>();

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

    selectClass(classId: string | undefined): void {
      if(!classId){
          this.actualSelectedClass = undefined;
            this.selectedClass.emit(undefined);
            return;
      }
      const classObj = this.classes.find(c => c.id === classId);
        if (this.actualSelectedClass === classObj) {
            this.actualSelectedClass = undefined;
            this.selectedClass.emit(undefined);
        } else {
            this.actualSelectedClass = classObj;
            this.selectedClass.emit(classObj?.id);
        }
    }

}
