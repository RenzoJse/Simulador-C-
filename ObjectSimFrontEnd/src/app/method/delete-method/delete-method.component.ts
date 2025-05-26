import { Component, OnInit } from '@angular/core';
import { CommonModule }            from '@angular/common';
import { Router }                  from '@angular/router';

import { MethodService }           from '../../../backend/services/method/method.service';
import { MethodListItem }          from '../../../backend/services/method/models/method-list-item.model';
import { DeleteMethodFormComponent } from '../../../business-components/delete-method/delete-method-form.component';

@Component({
  selector: 'app-delete-method',
  standalone: true,
  imports: [ CommonModule, DeleteMethodFormComponent ],
  templateUrl: './delete-method.component.html'
})
export class DeleteMethodComponent implements OnInit {
  methods: MethodListItem[] = [];
  status = {
    loading: false,
    error:   null as string | null
  };

  constructor(
    private methodService: MethodService,
    private router: Router
  ) {}

  ngOnInit() {
    this.methodService.getMethods().subscribe({
      next: list => this.methods = list,
      error: err => this.status.error = err.message || 'No se pudieron cargar métodos.'
    });
  }

  onDelete(id: string) {
    this.status.loading = true;
    this.status.error   = null;
    this.methodService.deleteMethod(id).subscribe({
      next:     () => this.router.navigate(['/']),
      error:    err => this.status.error = err.message || 'Error al eliminar.',
      complete: ()  => this.status.loading = false
    });
  }
}