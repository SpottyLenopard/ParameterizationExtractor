import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { HeaderComponent } from './screen/header.component';
import { FooterComponent } from './screen/footer.component';
import { apiService } from './services/apiService';

@NgModule({
    imports: [RouterModule],
    exports: [HeaderComponent, FooterComponent],
    declarations: [HeaderComponent, FooterComponent],
    providers: [apiService]
})
export class SharedModule { }