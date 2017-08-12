import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { HeaderComponent } from './screen/header.component';
import { FooterComponent } from './screen/footer.component';
import { AlertComponent } from './alert/alert.component';
import { apiService } from './services/apiService';
import { AlertService } from './services/alert.service';

@NgModule({
    imports: [RouterModule, CommonModule],
    exports: [HeaderComponent, FooterComponent, AlertComponent],
    declarations: [HeaderComponent, FooterComponent, AlertComponent],
    providers: [apiService, AlertService]
})
export class SharedModule { }