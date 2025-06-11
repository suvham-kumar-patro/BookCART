import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  template: `
    <div class="alert alert-danger mt-4">
      <h4>Unauthorized</h4>
      <p>{{ message }}</p>
    </div>
  `
})
export class UnauthorizedComponent {
  message: string = '';

  constructor(private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      this.message = params['message'] || 'Access denied.';
    });
  }
}
