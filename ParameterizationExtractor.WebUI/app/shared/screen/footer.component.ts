import { Component, OnInit } from '@angular/core';

@Component({
    moduleId: module.id,
    selector: 'footer-templ',
    templateUrl: './footer.component.html'
})
export class FooterComponent { today = new Date().toLocaleDateString(); }