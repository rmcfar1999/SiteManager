import { Component } from '@angular/core';

@Component({
  selector: 'app-branding',
  template: `
    <a class="matero-branding" href="#/">
      <img src="./assets/images/ssi_logo.png" class="matero-branding-logo-expanded" alt="logo" />
      <span class="matero-branding-name">Site Manager</span>
    </a>
  `,
})
export class BrandingComponent {}
